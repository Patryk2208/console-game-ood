using System.IO.Compression;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks.Sources;
using Model;
using Model.Commands;
using Model.Communication.Snapshots;

namespace GameServer.App;

public class ClientContext
{
    private long PlayerId { get; set; }
    private TcpClient Client { get; set; }
    private CancellationTokenSource Cts { get; set; }
    private Channel<GameSnapshot?> SnapshotsChannel { get; set; } //todo maybe possible without channels
    private Channel<Command> CommonCommandsChannel { get; set; }

    public ClientContext(long playerId, TcpClient client, CancellationTokenSource cts,
        Channel<Command> commandsChannel, Channel<GameSnapshot?> snapshotsChannel)
    {
        PlayerId = playerId;
        Client = client;
        Cts = cts;
        CommonCommandsChannel = commandsChannel;
        SnapshotsChannel = snapshotsChannel;
    }
    
    public async Task HandleClient()
    {
        var connections = new Task[2];
        Console.WriteLine($"Server - Connected to client {Client.Client.RemoteEndPoint}");
        try
        {
            using (Client)
            {
                await using (var netStream = Client.GetStream())
                {
                    connections[0] = RunServerToClientConnection(netStream);
                    connections[1] = RunClientToServerInput(netStream);
                    Task.WaitAll(connections);
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await Cts.CancelAsync();
        }
    }
    
    private async Task RunServerToClientConnection(NetworkStream stream)
    {
        await stream.WriteAsync(BitConverter.GetBytes(PlayerId), Cts.Token);
        while (!Cts.IsCancellationRequested)
        {
            var gameSnapshot = await SnapshotsChannel.Reader.ReadAsync(Cts.Token);
            
            if (gameSnapshot == null)
            {
                await Cts.CancelAsync();
                throw new Exception("null snapshot received, client disconnected");
            }
            
            await PrepareAndWriteCompressedMessage(stream, gameSnapshot, CompressionLevel.SmallestSize);
            
            Console.WriteLine($"Server - Player {PlayerId} was sent an update");
        }
    }
    private async Task PrepareAndWriteCompressedMessage(NetworkStream stream, GameSnapshot gameSnapshot, CompressionLevel compressionLevel)
    {
        var json = JsonSerializer.Serialize(gameSnapshot);
        var serializedJson = JsonSerializer.SerializeToUtf8Bytes(gameSnapshot);
        var preCompressedLength = serializedJson.Length;
        using var ms = new MemoryStream();
        await using(var gzip = new GZipStream(ms, compressionLevel))
        {
            await gzip.WriteAsync(serializedJson, Cts.Token);
        }
        var compressedBuffer = ms.ToArray();
        var compressedLength = compressedBuffer.Length;
        await stream.WriteAsync(BitConverter.GetBytes(compressedLength), Cts.Token);
        await stream.WriteAsync(BitConverter.GetBytes(preCompressedLength), Cts.Token);
        await stream.WriteAsync(compressedBuffer, 0, compressedLength, Cts.Token);
    }

    private async Task RunClientToServerInput(NetworkStream stream)
    {
        while (!Cts.IsCancellationRequested)
        {
            var receivedCommand = await ReceiveAndDecompressCommand(stream);

            Console.WriteLine($"Server - Received command from Player {PlayerId}");

            await CommonCommandsChannel.Writer.WriteAsync(receivedCommand, Cts.Token);

            if (receivedCommand.GetType() == typeof(ExitCommand))
            {
                await Cts.CancelAsync();
            }
        }
    }
    
    private async Task<Command> ReceiveAndDecompressCommand(NetworkStream stream)
    {
        var msgCompressedLen = new byte[4];
        var msgPreCompressedLen = new byte[4];
        await stream.ReadExactlyAsync(msgCompressedLen, 0, sizeof(int), Cts.Token);
        var compressedLen = BitConverter.ToInt32(msgCompressedLen, 0);
        await stream.ReadExactlyAsync(msgPreCompressedLen, 0, sizeof(int), Cts.Token);
        var preCompressedLen = BitConverter.ToInt32(msgPreCompressedLen, 0);
        
        var compressedBuffer = new byte[compressedLen];
        await stream.ReadExactlyAsync(compressedBuffer, 0, compressedLen, Cts.Token);

        using var msOut = new MemoryStream();
        byte[] decompressedBuffer;
        using (var ms = new MemoryStream(compressedBuffer, 0, compressedLen))
        {
            await using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
            {
                await gzip.CopyToAsync(msOut, compressedLen, Cts.Token);
                decompressedBuffer = msOut.ToArray();
            }
        }

        var recCommand = JsonSerializer.Deserialize<Command>(decompressedBuffer);

        if (recCommand == null) throw new Exception("Json deserialization error");
        return recCommand;
    }
}
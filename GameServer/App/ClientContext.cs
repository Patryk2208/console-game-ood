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
    private CancellationToken Ct { get; set; }
    private MvcSynchronization Mvc { get; set; }
    private Channel<GameSnapshot?> SnapshotsChannel { get; set; } //todo maybe possible without channels
    private Channel<Command> CommonCommandsChannel { get; set; }

    public ClientContext(long playerId, TcpClient client, CancellationToken ct, MvcSynchronization mvc,
        Channel<Command> commandsChannel, Channel<GameSnapshot?> snapshotsChannel)
    {
        PlayerId = playerId;
        Client = client;
        Ct = ct;
        Mvc = mvc;
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
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        Task.WaitAll(connections, Ct);
    }
    
    private async Task RunServerToClientConnection(NetworkStream stream)
    {
        await stream.WriteAsync(BitConverter.GetBytes(PlayerId), Ct);
        while (!Mvc.ShouldAllExit)
        {
            var gameSnapshot = await SnapshotsChannel.Reader.ReadAsync(Ct);
            
            if (gameSnapshot == null)
            {
                //todo
                return;
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
            await gzip.WriteAsync(serializedJson, Ct);
        }
        var compressedBuffer = ms.ToArray();
        var compressedLength = compressedBuffer.Length;
        await stream.WriteAsync(BitConverter.GetBytes(compressedLength), Ct);
        await stream.WriteAsync(BitConverter.GetBytes(preCompressedLength), Ct);
        await stream.WriteAsync(compressedBuffer, 0, compressedLength, Ct);
    }

    private async Task RunClientToServerInput(NetworkStream stream)
    {
        while (!Mvc.ShouldAllExit)
        {
            var receivedCommand = await ReceiveAndDecompressCommand(stream);

            Console.WriteLine($"Server - Received command from Player {PlayerId}");

            await CommonCommandsChannel.Writer.WriteAsync(receivedCommand, Ct);

            if (receivedCommand.GetType() == typeof(ExitCommand))
            {
                //todo
                break;
            }
        }
    }
    
    private async Task<Command> ReceiveAndDecompressCommand(NetworkStream stream)
    {
        var msgCompressedLen = new byte[4];
        var msgPreCompressedLen = new byte[4];
        await stream.ReadExactlyAsync(msgCompressedLen, 0, sizeof(int), Ct);
        var compressedLen = BitConverter.ToInt32(msgCompressedLen, 0);
        await stream.ReadExactlyAsync(msgPreCompressedLen, 0, sizeof(int), Ct);
        var preCompressedLen = BitConverter.ToInt32(msgPreCompressedLen, 0);
        
        var compressedBuffer = new byte[compressedLen];
        await stream.ReadExactlyAsync(compressedBuffer, 0, compressedLen, Ct);

        using var msOut = new MemoryStream();
        byte[] decompressedBuffer;
        using (var ms = new MemoryStream(compressedBuffer, 0, compressedLen))
        {
            await using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
            {
                await gzip.CopyToAsync(msOut, compressedLen, Ct);
                decompressedBuffer = msOut.ToArray();
            }
        }

        var recCommand = JsonSerializer.Deserialize<Command>(decompressedBuffer);

        if (recCommand == null) throw new Exception("Json deserialization error");
        return recCommand;
    }
}
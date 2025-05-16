using System.Buffers.Binary;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Channels;
using RPG_ood.Commands;
using RPG_ood.Communication.Snapshots;
using RPG_ood.Controller.Input;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game;
using RPG_ood.Model.Game.GameState;
using RPG_ood.View.Display;

namespace RPG_ood.App.Server;

public class Server
{
    private GameState TotalState { get; init; }
    private Input.Input Controller { get; init; }
    private MvcSynchronization Mvc { get; init; }
    private Dictionary<long, (Task, Task)> ConnectedClients { get; init; }
    private List<long> ConnectedClientIds { get; init; }
    private Mutex ConnectionMutex { get; init; }
    private Dictionary<long, Channel<GameSnapshot>> Channels { get; init; }
    private Channel<Command> CommandsChannel { get; init; }
    private Mutex CommandMutex { get; init; }
    private const int MaxClients = 10;
    private int Port { get; set; }
    
    public Server(int port = 5555)
    {
        CommandsChannel = Channel.CreateUnbounded<Command>();
        CommandMutex = new Mutex();
        Mvc = new MvcSynchronization();
        Controller = new Input.Input(Mvc, CommandsChannel);
        Channels = new ();
        TotalState = new GameState(Mvc, Controller, Channels);
        Port = port;
        ConnectedClients = new();
        ConnectedClientIds = new();
        ConnectionMutex = new Mutex();
    }

    public async Task Run()
    {
        var gameAndConnections = new Task[3];
        gameAndConnections[0] = TotalState.RunGame();
        gameAndConnections[1] = Controller.RunInput();
        gameAndConnections[2] = ManageConnections();
        await Task.WhenAll(gameAndConnections);
    }
    
    private async Task ManageConnections()
    {
        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();

        try
        {
            while (!Mvc.ShouldAllExit)
            {
                var newClient = await listener.AcceptTcpClientAsync();
                var cts = new CancellationTokenSource();
                ConnectionMutex.WaitOne();
                if (ConnectedClientIds.Count >= MaxClients)
                {
                    ConnectionMutex.ReleaseMutex();
                    using (var stream = newClient.GetStream())
                    {
                        byte[] denyMsg = new byte[sizeof(long)];
                        BinaryPrimitives.WriteInt64LittleEndian(denyMsg, -1);
                        await stream.WriteAsync(denyMsg);
                    }
                    newClient.Close();
                    continue;
                }
                var id = GenerateNewPlayerId(newClient);
                ConnectedClientIds.Add(id);
                Mvc.GameMutex.WaitOne();
                TotalState.AddPlayer(id);
                Mvc.GameMutex.ReleaseMutex();
                var snapshots = RunServerToClientConnection(id, newClient, cts);
                var commands = RunClientToServerInput(id, newClient, cts);
                ConnectedClients.Add(id, (snapshots, commands));
                ConnectionMutex.ReleaseMutex();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            foreach (var (clientInput, clientOutput) in ConnectedClients.Values)
            {
                clientInput.Dispose();
                clientOutput.Dispose();
            }
            await Task.WhenAll(ConnectedClients.SelectMany(t => new []{t.Value.Item1, t.Value.Item2}));
        
            listener.Stop();
        }
    }
    
    private async Task RunServerToClientConnection(long playerId, TcpClient client, CancellationTokenSource cts)
    {
        try
        {
            using (client)
            {
                Console.WriteLine($"Server - Connected to client {client.Client.RemoteEndPoint}");
                await using (var stream = client.GetStream())
                {
                    await stream.WriteAsync(BitConverter.GetBytes(playerId));
                    while (!Mvc.ShouldAllExit)
                    {
                        if (!Channels.ContainsKey(playerId)) throw new Exception("Player disconnected");
                        var gameSnapshot = await Channels[playerId].Reader.ReadAsync();
                        
                        await PrepareAndWriteCompressedMessage(stream, gameSnapshot, CompressionLevel.SmallestSize);
                        
                        Console.WriteLine($"Server - Player {playerId} was sent an update");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            client.Close();
            Console.WriteLine($"Player {playerId} disconnected");
        }
    }
    private async Task PrepareAndWriteCompressedMessage(NetworkStream stream, GameSnapshot gameSnapshot, CompressionLevel compressionLevel)
    {
        var serializedJson = JsonSerializer.SerializeToUtf8Bytes(gameSnapshot);
        var preCompressedLength = serializedJson.Length;
        using var ms = new MemoryStream();
        await using(var gzip = new GZipStream(ms, compressionLevel))
        {
            await gzip.WriteAsync(serializedJson);
        }
        var compressedBuffer = ms.ToArray();
        var compressedLength = compressedBuffer.Length;
        await stream.WriteAsync(BitConverter.GetBytes(compressedLength));
        await stream.WriteAsync(BitConverter.GetBytes(preCompressedLength));
        await stream.WriteAsync(compressedBuffer, 0, compressedLength);
    }

    private async Task RunClientToServerInput(long playerId, TcpClient client, CancellationTokenSource cts)
    {
        try
        {
            using (client)
            {
                await using (var stream = client.GetStream())
                {
                    while (!Mvc.ShouldAllExit)
                    {
                        var receivedCommand = await ReceiveAndDecompressCommand(stream);

                        Console.WriteLine($"Server - Received command from Player {playerId}");

                        CommandMutex.WaitOne();
                        await CommandsChannel.Writer.WriteAsync(receivedCommand);
                        CommandMutex.ReleaseMutex();
                        
                        if (receivedCommand.KeyInfo.Key != ConsoleKey.Escape) continue;
                        await cts.CancelAsync();
                        client.Close();
                        ConnectionMutex.WaitOne();
                        ConnectedClients.Remove(playerId);
                        ConnectedClientIds.Remove(playerId);
                        Channels.Remove(playerId);
                        ConnectionMutex.ReleaseMutex();
                        return;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            client.Close();
            Console.WriteLine($"Player {playerId} disconnected");
        }
    }
    
    private async Task<Command> ReceiveAndDecompressCommand(NetworkStream stream)
    {
        var msgCompressedLen = new byte[4];
        var msgPreCompressedLen = new byte[4];
        await stream.ReadExactlyAsync(msgCompressedLen, 0, sizeof(int));
        var compressedLen = BitConverter.ToInt32(msgCompressedLen, 0);
        await stream.ReadExactlyAsync(msgPreCompressedLen, 0, sizeof(int));
        var preCompressedLen = BitConverter.ToInt32(msgPreCompressedLen, 0);
        
        var compressedBuffer = new byte[compressedLen];
        await stream.ReadExactlyAsync(compressedBuffer, 0, compressedLen);

        using var msOut = new MemoryStream();
        byte[] decompressedBuffer;
        using (var ms = new MemoryStream(compressedBuffer, 0, compressedLen))
        {
            await using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
            {
                gzip.CopyTo(msOut, compressedLen);
                decompressedBuffer = msOut.ToArray();
            }
        }

        var recCommand = JsonSerializer.Deserialize<Command>(decompressedBuffer);

        if (recCommand == null) throw new Exception("Json deserialization error");
        return recCommand;
    }
    
    private long GenerateNewPlayerId(TcpClient client)
    {
        var endPoint = client.Client.RemoteEndPoint;
        if (endPoint == null) throw new NullReferenceException();
        var strEnd = endPoint.ToString();
        var ipPort = strEnd.Split(':');
        var ip = ipPort[0].Split('.');
        long id = 0;
        for (int i = 0; i < ip.Length; i++)
        {
            int part = int.Parse(ip[i]);
            id += part;
            id <<= 8;
        }
        id <<= 32;
        int port = int.Parse(ipPort[1]);
        id += port;
        return id;
    }
}
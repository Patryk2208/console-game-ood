using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Channels;
using Agones;
using Agones.Dev.Sdk;
using GameServer.Controller;

using Model;
using Model.Commands;
using Model.Communication.Snapshots;
using Model.Game.GameState;

namespace GameServer.App;

public class Server
{
    private Input Controller { get; init; }
    private MvcSynchronization Mvc { get; init; }
    private ConcurrentDictionary<long, ClientContext> ConnectedClients { get; init; }
    private Mutex ConnectionMutex { get; init; }
    private ConcurrentDictionary<long, Channel<GameSnapshot?>> Channels { get; init; }
    private Channel<Command> CommandsChannel { get; init; }
    private int Port { get; set; }
    private AgonesSDK AgonesSdk { get; init; }
    private long PlayerCapacity { get; set; }

    public Server()
    {
        CommandsChannel = Channel.CreateUnbounded<Command>();
        Channels = new ConcurrentDictionary<long, Channel<GameSnapshot?>>();
        ConnectionMutex = new Mutex();
        Mvc = new MvcSynchronization();
        Controller = new Input(Mvc, CommandsChannel, Channels, ConnectionMutex);
        Port = int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "7777");
        ConnectedClients = new ConcurrentDictionary<long, ClientContext>();
        AgonesSdk = new AgonesSDK();
    }

    public async Task Run()
    {
        Console.WriteLine("Server started");

        var gameAndConnections = new Task[3];
        gameAndConnections[0] = Controller.RunGame();
        gameAndConnections[1] = Controller.RunInput();
        gameAndConnections[2] = ManageConnections();
        await Task.WhenAll(gameAndConnections);
        await AgonesSdk.ShutDownAsync();
    }

    private async Task HealthChecker()
    {
        while (Mvc.ShouldAllExit == false)
        {
            var hs = await AgonesSdk.HealthAsync();
            await Task.Delay(3000, Mvc.ImmediateExit.Token);
        }
    }

    private async Task ManageConnections()
    {
        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();
        Console.WriteLine($"Listening on port {Port}");
        try
        {
            var status = await AgonesSdk.ReadyAsync();
            HealthChecker();
            PlayerCapacity = await AgonesSdk.Alpha().GetPlayerCapacityAsync();

            while (!Mvc.ShouldAllExit)
            {
                var newClient = await listener.AcceptTcpClientAsync(Mvc.ImmediateExit.Token);
                await AddPlayer(newClient);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            listener.Stop();
        }
    }

    private async Task AddPlayer(TcpClient newClient)
    {
        ConnectionMutex.WaitOne();
        var id = GenerateNewPlayerId(newClient);
        var connectSuccess = await AgonesSdk.Alpha().PlayerConnectAsync(id.ToString());
        var pCh = Channel.CreateUnbounded<GameSnapshot?>();
        if (!Channels.TryAdd(id, pCh) || !ConnectedClients.TryAdd(id,
                new ClientContext(id, newClient, Mvc.ImmediateExit.Token, Mvc, CommandsChannel, pCh)))
        {
            ConnectionMutex.ReleaseMutex();
            Console.WriteLine("Failed to add player Server shutting down");
            throw new Exception();
        }

        if (!connectSuccess)
        {
            ConnectionMutex.ReleaseMutex();
            await using (var stream = newClient.GetStream())
            {
                var denyMsg = new byte[sizeof(long)];
                BinaryPrimitives.WriteInt64LittleEndian(denyMsg, -1);
                await stream.WriteAsync(denyMsg, Mvc.ImmediateExit.Token);
            }

            newClient.Close();
            return;
        }

        Task.Run(async () =>
        {
            await ConnectedClients[id].HandleClient();
            await RemovePlayer(id);
        });

        Mvc.GameMutex.WaitOne();
        Controller.AddPlayer(id);
        Mvc.GameMutex.ReleaseMutex();

        Console.WriteLine($"Player {id} connected");

        ConnectionMutex.ReleaseMutex();
    }

    private async Task RemovePlayer(long id)
    {
        var disconnectSuccess = await AgonesSdk.Alpha().PlayerDisconnectAsync(id.ToString());
        var removeChannelSuccess = Channels.TryRemove(id, out _);
        var clientSuccess = ConnectedClients.TryRemove(id, out _);

        if (!(disconnectSuccess && removeChannelSuccess && clientSuccess))
        {
            Console.WriteLine("Failed to disconnect player Server shutting down");
            throw new Exception();
        }

        Mvc.GameMutex.WaitOne();
        Controller.RemovePlayer(id);
        Mvc.GameMutex.ReleaseMutex();
        Console.WriteLine($"Player {id} disconnected");
        CheckGameEnd();
    }

    private void CheckGameEnd()
    {
        var count = ConnectedClients.Count;
        if (count == 0)
        {
            //todo
        }
    }

private long GenerateNewPlayerId(TcpClient client)
    {
        var endPoint = client.Client.RemoteEndPoint;
        if (endPoint == null) throw new NullReferenceException();
        var strEnd = endPoint.ToString();
        if (strEnd == null) throw new NullReferenceException();
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
        var port = int.Parse(ipPort[1]);
        id += port;
        return id;
    }
}
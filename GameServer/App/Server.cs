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
    private SemaphoreSlim ConnectionSemaphore { get; init; }
    private ConcurrentDictionary<long, Channel<GameSnapshot?>> Channels { get; init; }
    private Channel<Command> CommandsChannel { get; init; }
    private int Port { get; set; }
    private AgonesSDK AgonesSdk { get; init; }
    private long PlayerCapacity { get; set; }

    public Server()
    {
        CommandsChannel = Channel.CreateUnbounded<Command>();
        Channels = new ConcurrentDictionary<long, Channel<GameSnapshot?>>();
        ConnectionSemaphore = new SemaphoreSlim(1, 1);
        Mvc = new MvcSynchronization();
        Controller = new Input(Mvc, CommandsChannel, Channels, ConnectionSemaphore);
        Port = int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "7777");
        ConnectedClients = new ConcurrentDictionary<long, ClientContext>();
        PlayerCapacity = 10;
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
        while (!Mvc.ImmediateExit.IsCancellationRequested)
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
            await AgonesSdk.Alpha().SetPlayerCapacityAsync(PlayerCapacity);
            PlayerCapacity = await AgonesSdk.Alpha().GetPlayerCapacityAsync();
            Console.WriteLine($"Server ready, player capacity: {PlayerCapacity}");
            while (!Mvc.ImmediateExit.IsCancellationRequested)
            {
                var newClient = await listener.AcceptTcpClientAsync(Mvc.ImmediateExit.Token);
                await AddPlayer(newClient);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            if (ConnectionSemaphore.CurrentCount == 0)
            {
                ConnectionSemaphore.Release();
            }
            Console.WriteLine("Manage connections closes correctly");
            await Mvc.ImmediateExit.CancelAsync();
        }
        finally
        {
            listener.Stop();
        }
    }

    private async Task AddPlayer(TcpClient newClient)
    {
        var id = GenerateNewPlayerId(newClient);
        await ConnectionSemaphore.WaitAsync(Mvc.ImmediateExit.Token);
        var connectSuccess = await AgonesSdk.Alpha().PlayerConnectAsync(id.ToString());
        var pCh = Channel.CreateUnbounded<GameSnapshot?>();
        if (!Channels.TryAdd(id, pCh) || !ConnectedClients.TryAdd(id,
                new ClientContext(id, newClient, new CancellationTokenSource(), CommandsChannel, pCh)))
        {
            ConnectionSemaphore.Release();
            Console.WriteLine("Failed to add player Server shutting down");
            throw new Exception();
        }

        if (!connectSuccess)
        {
            ConnectionSemaphore.Release();
            await using (var stream = newClient.GetStream())
            {
                var denyMsg = new byte[sizeof(long)];
                BinaryPrimitives.WriteInt64LittleEndian(denyMsg, -1);
                await stream.WriteAsync(denyMsg, Mvc.ImmediateExit.Token);
            }

            newClient.Close();
            return;
        }

        Mvc.GameMutex.WaitOne();
        Controller.AddPlayer(id);
        Mvc.GameMutex.ReleaseMutex();
        ConnectionSemaphore.Release();
        
        Task.Run(async () =>
        {
            await ConnectedClients[id].HandleClient();
            await RemovePlayer(id);
        });
        
        Console.WriteLine($"Player {id} connected");
    }

    private async Task RemovePlayer(long id)
    {
        await ConnectionSemaphore.WaitAsync(Mvc.ImmediateExit.Token);
        var disconnectSuccess = await AgonesSdk.Alpha().PlayerDisconnectAsync(id.ToString());
        var removeChannelSuccess = Channels.TryRemove(id, out _);
        var clientSuccess = ConnectedClients.TryRemove(id, out _);

        if (!(disconnectSuccess && removeChannelSuccess && clientSuccess))
        {
            ConnectionSemaphore.Release();
            Console.WriteLine("Failed to disconnect player Server shutting down");
            throw new Exception();
        }
        ConnectionSemaphore.Release();
        Console.WriteLine($"Player {id} disconnected");
        CheckGameEnd();
    }

    private void CheckGameEnd()
    {
        ConnectionSemaphore.Wait();
        var count = ConnectedClients.Count;
        if (count == 0)
        {
            Console.WriteLine("Ending gameserver");
            Mvc.ImmediateExit.Cancel();
        }
        ConnectionSemaphore.Release();
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
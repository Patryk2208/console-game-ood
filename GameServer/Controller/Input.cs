using System.Threading.Channels;
using Model;
using Model.Commands;
using Model.Game.GameState;
using Model.Communication.Snapshots;

namespace GameServer.Controller;

public class Input
{
    private GameState State { get; init; }
    private MvcSynchronization Sync { get; set; }
    private Channel<Command> CommandsChannel { get; set; }
    private bool WasStateChangedBetweenMoments { get; set; }
    private SemaphoreSlim ResponsivenessSemaphore { get; set; }
    private Dictionary<long, Channel<GameSnapshot?>> PlayerChannels { get; set; }
    private Mutex ConnectionMutex { get; set; }

    public Input(GameState state, MvcSynchronization sync, Channel<Command> commandsChannel,
        Dictionary<long, Channel<GameSnapshot?>> playerChannels, Mutex connectionMutex)
    {
        State = state;
        Sync = sync;
        CommandsChannel = commandsChannel;
        WasStateChangedBetweenMoments = false;
        ResponsivenessSemaphore = new SemaphoreSlim(0, 1);
        PlayerChannels = playerChannels;
        ConnectionMutex = connectionMutex;
    }
    public async Task RunGame()
    {
        try
        {
            var cts = new CancellationTokenSource();
            var respMoment = Math.Min(State.MomentDurationMilliseconds, 10);
            var resp = ResponsivenessMaintainer(respMoment, cts);
            while (Sync.ShouldExitModel == false)
            {
                await Task.Delay(State.MomentDurationMilliseconds);

                ConnectionMutex.WaitOne();
                Sync.GameMutex.WaitOne();

                await BroadcastStates();

                State.MomentChangedEvent.NotifyObservers(State, 0);
                ++State.CurrentMoment;

                Sync.GameMutex.ReleaseMutex();
                ConnectionMutex.ReleaseMutex();
            }

            cts.Cancel();
            resp.Wait(cts.Token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task ResponsivenessMaintainer(int respMoment, CancellationTokenSource cts)
    {
        try
        {
            while (cts.IsCancellationRequested == false)
            {
                await Task.Delay(respMoment);

                await ResponsivenessSemaphore.WaitAsync();

                ConnectionMutex.WaitOne();
                Sync.GameMutex.WaitOne();
                if (WasStateChangedBetweenMoments)
                {
                    WasStateChangedBetweenMoments = false;
                    await BroadcastStates();
                }

                Sync.GameMutex.ReleaseMutex();
                ConnectionMutex.ReleaseMutex();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task RunInput()
    {
        try
        {
            while (!Sync.ShouldExitController)
            {
                var command = await CommandsChannel.Reader.ReadAsync();
                Sync.GameMutex.WaitOne();
                command.Execute(State);

                WasStateChangedBetweenMoments = true;
                if (ResponsivenessSemaphore.CurrentCount == 0)
                {
                    ResponsivenessSemaphore.Release();
                }

                Sync.GameMutex.ReleaseMutex();
                Console.WriteLine($"Server executed command {command.GetType().Name}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void AddPlayer(long playerId)
    {
        State.AddPlayer(playerId);
        var channel = Channel.CreateUnbounded<GameSnapshot?>();
        PlayerChannels.Add(playerId, channel);
    }
    
    private async Task BroadcastStates()
    {
        foreach (var channel in PlayerChannels)
        {
            GameSnapshot? relativeState = null;
            if (State.Players.ContainsKey(channel.Key))
            {
                relativeState = new GameSnapshot(State, channel.Key);  
            }
            await channel.Value.Writer.WriteAsync(relativeState);
        }
    }
}
using System.Collections.Concurrent;
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
    private ConcurrentDictionary<long, Channel<GameSnapshot?>> PlayerChannels { get; set; }
    private Mutex ConnectionMutex { get; set; }

    public Input(MvcSynchronization sync, Channel<Command> commandsChannel,
        ConcurrentDictionary<long, Channel<GameSnapshot?>> playerChannels, Mutex connectionMutex)
    {
        State = new GameState(sync);
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
            while (!Sync.ImmediateExit.IsCancellationRequested)
            {
                await Task.Delay(State.MomentDurationMilliseconds, Sync.ImmediateExit.Token);

                ConnectionMutex.WaitOne();
                Sync.GameMutex.WaitOne();

                await BroadcastStates();

                State.MomentChangedEvent.NotifyObservers(State, 0);
                ++State.CurrentMoment;

                Sync.GameMutex.ReleaseMutex();
                ConnectionMutex.ReleaseMutex();
            }

            await cts.CancelAsync();
            await resp.WaitAsync(cts.Token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await Sync.ImmediateExit.CancelAsync();
        }
    }

    private async Task ResponsivenessMaintainer(int respMoment, CancellationTokenSource cts)
    {
        try
        {
            while (!Sync.ImmediateExit.IsCancellationRequested)
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
            while (!Sync.ImmediateExit.IsCancellationRequested)
            {
                var command = await CommandsChannel.Reader.ReadAsync(Sync.ImmediateExit.Token);
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
            await Sync.ImmediateExit.CancelAsync();
        }
    }

    public void AddPlayer(long playerId)
    {
        State.AddPlayer(playerId);
    }
    public void RemovePlayer(long playerId)
    {
        State.RemovePlayer(playerId);
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
            await channel.Value.Writer.WriteAsync(relativeState, Sync.ImmediateExit.Token);
        }
    }
}
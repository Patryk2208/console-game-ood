using System.Threading.Channels;
using RPG_ood.App;
using RPG_ood.Commands;
using RPG_ood.Model.Game.GameState;

namespace RPG_ood.Input;

public class Input
{
    private GameState State { get; init; }
    private MvcSynchronization Sync { get; set; }
    private Channel<Command> CommandsChannel { get; set; }
    private bool WasStateChangedBetweenMoments { get; set; }
    private SemaphoreSlim ResponsivenessSemaphore { get; set; }

    public Input(GameState state, MvcSynchronization sync, Channel<Command> commandsChannel)
    {
        State = state;
        Sync = sync;
        CommandsChannel = commandsChannel;
        WasStateChangedBetweenMoments = false;
        ResponsivenessSemaphore = new SemaphoreSlim(0, 1);
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

                State.ConnectionMutex.WaitOne();
                Sync.GameMutex.WaitOne();

                await State.BroadcastStates();

                State.MomentChangedEvent.NotifyObservers(State, 0);
                ++State.CurrentMoment;

                Sync.GameMutex.ReleaseMutex();
                State.ConnectionMutex.ReleaseMutex();
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

                State.ConnectionMutex.WaitOne();
                Sync.GameMutex.WaitOne();
                if (WasStateChangedBetweenMoments)
                {
                    WasStateChangedBetweenMoments = false;
                    await State.BroadcastStates();
                }

                Sync.GameMutex.ReleaseMutex();
                State.ConnectionMutex.ReleaseMutex();
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
}
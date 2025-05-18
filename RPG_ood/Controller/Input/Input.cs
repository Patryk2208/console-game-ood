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
    public Input(GameState state, MvcSynchronization sync, Channel<Command> commandsChannel)
    {
        State = state;
        Sync = sync;
        CommandsChannel = commandsChannel;
    }

    public async Task RunInput()
    {
        while (!Sync.ShouldExitController)
        {
            var command = await CommandsChannel.Reader.ReadAsync();
            Sync.GameMutex.WaitOne();
            command.Execute(State);
            Sync.GameMutex.ReleaseMutex();
            Console.WriteLine($"Server executed command {nameof(command)}.");
        }
    }
    
    /*public void TakeInput()
    {
        Task.Run(
            () =>
            {
                while (Sync.ShouldExitController == false)
                {
                    var key = Console.ReadKey(true);
                    Sync.GameMutex.WaitOne();
                    ChainOfResponsibilityHandler.HandleInput(key);
                    Sync.GameMutex.ReleaseMutex();
                }
            });
    }*/
}
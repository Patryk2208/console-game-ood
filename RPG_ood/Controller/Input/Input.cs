using System.Threading.Channels;
using RPG_ood.App;
using RPG_ood.Commands;
using RPG_ood.Controller.Input;

namespace RPG_ood.Input;

public class Input
{
    public ConsoleInputHandlerLink ChainOfResponsibilityHandler { private get; set; }
    private MvcSynchronization Sync { get; set; }
    private Channel<Command> CommandsChannel { get; set; }
    public Input(MvcSynchronization sync, Channel<Command> commandsChannel)
    {
        Sync = sync;
        CommandsChannel = commandsChannel;
        ChainOfResponsibilityHandler = null!;
    }

    public async Task RunInput()
    {
        while (!Sync.ShouldExitController)
        {
            var command = await CommandsChannel.Reader.ReadAsync();
            Sync.GameMutex.WaitOne();
            ChainOfResponsibilityHandler.HandleInput(command);
            Sync.GameMutex.ReleaseMutex();
            Console.WriteLine($"Server executed command {command.KeyInfo.Key.ToString()}.");
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
using RPG_ood.Commands;

namespace RPG_ood.Input;

public class Controller
{
    private ConsoleInputHandlerLink HandlerChainHead { get; set; }

    public Controller()
    {
        var builder = new InputHandlingBuilder();
        builder.BuildAll();
        HandlerChainHead = builder.GetResult();
    }

    public Command? ParseInputIntoCommand(InputUnit iu)
    {
        HandlerChainHead.HandleInput(iu, out var command);
        return command;
    }
}
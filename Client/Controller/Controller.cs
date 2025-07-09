using Model.Commands;

namespace Client.Controller;

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
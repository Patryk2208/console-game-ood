using Model.Commands;

namespace Client.Controller;

public abstract class ConsoleInputHandlerLink
{
    protected ConsoleInputHandlerLink NextLink { get; set; }
    public void SetNextLink(ConsoleInputHandlerLink nextLink)
    {
        NextLink = nextLink;
    }
    public abstract void HandleInput(InputUnit iu, out Command? command);
}

/*public class VerifyUserLink(GameState state) : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(InputUnit iu)
    {
        if (State.Players.ContainsKey(iu.PlayerId) && !State.Players[iu.PlayerId].IsDead)
        {
            NextLink.HandleInput(iu);
        }
        else
        {
            State.Logs.AddLogMessage("Player with specified id does not exist or is dead");
        }
    }
}*/

public class MoveUpLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.W)
        {
            command = new MoveUpCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class MoveDownLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.S)
        {
            command = new MoveDownCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class MoveLeftLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.A)
        {
            command = new MoveLeftCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class MoveRightLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.D)
        {
            command = new MoveRightCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class EquipLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.E)
        {
            command = new EquipCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class ThrowLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.T)
        {
            command = new ThrowCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class ThrowAllLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.Y)
        {
            command = new ThrowAllCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class EqSelectDownLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.LeftArrow)
        {
            command = new EqSelectDownCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class EqSelectUpLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.RightArrow)
        {
            command = new EqSelectUpCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class PickUpSelectDownLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.DownArrow)
        {
            command = new PickUpSelectDownCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class PickUpSelectUpLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.UpArrow)
        {
            command = new PickUpSelectUpCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class PutInRightHandLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.P)
        {
            command = new PutInRightHandCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class PutInLeftHandLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.L)
        {
            command = new PutInLeftHandCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class UseFromLeftHandLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.U && ((int)iu.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1)
        {
            command = new UseFromLeftHandCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class UseFromRightHandLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.U && ((int)iu.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0)
        {
            command = new UseFromRightHandCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class NormalAttackRightHandLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.Enter && ((int)iu.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0)
        {
            command = new NormalAttackRightHandCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class NormalAttackLeftHandLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.Enter && ((int)iu.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1)
        {
            command = new NormalAttackLeftHandCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class SneakAttackRightHandLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.Backspace && ((int)iu.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0)
        {
            command = new SneakAttackRightHandCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class SneakAttackLeftHandLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.Backspace && ((int)iu.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1)
        {
            command = new SneakAttackLeftHandCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class MagicAttackRightHandLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.Delete && ((int)iu.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0)
        {
            command = new MagicAttackRightHandCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class MagicAttackLeftHandLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.Delete && ((int)iu.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1)
        {
            command = new MagicAttackLeftHandCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class ExitLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        if (iu.KeyInfo.Key == ConsoleKey.Escape)
        {
            command = new ExitCommand(iu.PlayerId);
        }
        else
        {
            NextLink.HandleInput(iu, out command);
        }
    }
}

public class SentinelLink : ConsoleInputHandlerLink
{
    public override void HandleInput(InputUnit iu, out Command? command)
    {
        command = null;
    }
}
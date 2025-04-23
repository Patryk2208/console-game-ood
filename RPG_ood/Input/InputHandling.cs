using RPG_ood.Attack;
using RPG_ood.Game;
using RPG_ood.Beings;

namespace RPG_ood.Input;

public abstract class ConsoleInputHandlerLink (GameState state, Logs logs)
{
    public ConsoleInputHandlerLink NextLink { get; set; }
    protected GameState State { get; } = state;
    protected Logs Logs { get; } = logs;
    public abstract void HandleInput(ConsoleKeyInfo keyInfo);
}

public class MoveUpLink(GameState state,  Logs logs) 
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.W)
        {
            State.Player.MoveUp(State.CurrentRoom);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class MoveDownLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.S)
        {
            State.Player.MoveDown(State.CurrentRoom);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class MoveLeftLink(GameState state, Logs logs) 
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.A)
        {
            State.Player.MoveLeft(State.CurrentRoom);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class MoveRightLink(GameState state, Logs logs) 
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.D)
        {
            State.Player.MoveRight(State.CurrentRoom);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class EquipLink(GameState state, Logs logs) 
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.E)
        {
            var itemsAtPos = State.CurrentRoom.GetItemsAtPos(State.Player.Pos).ToList();
            if (!itemsAtPos.Any()) return;
            var item = itemsAtPos.ElementAt(State.CurrentRoom.PickUpCursor);
            var newMessage = $"{State.Player.Name} picked up {item.Name} at {item.Pos}";
            item.Pos = item.Pos with { X = -1, Y = -1 };
            State.Player.PickUpItem(item);
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class ThrowLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.T)
        {
            if (State.Player.Eq.Eq.Count <= 0) return;
            var newMessage = $"{State.Player.Name} dropped {State.Player.Eq.Eq[State.Player.Eq.EqPointer].Name}";
            State.Player.DropItem();
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}



public class EqSelectDownLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.LeftArrow)
        {
            State.Player.Eq.TryMovePointerLeft();
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class ThrowAllLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.Y)
        {
            if (State.Player.Eq.Eq.Count <= 0) return;
            var newMessage = $"{State.Player.Name} dropped all items";
            while (State.Player.Eq.Eq.Count > 0)
            {
                State.Player.DropItem();
            }
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class EqSelectUpLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.RightArrow)
        {
            State.Player.Eq.TryMovePointerRight();
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class PickUpSelectDownLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.DownArrow)
        {
            State.CurrentRoom.PickUpCursor++;
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class PickUpSelectUpLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.UpArrow)
        {
            State.CurrentRoom.PickUpCursor = Math.Max(0, State.CurrentRoom.PickUpCursor - 1);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class PutInRightHand(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.P)
        {
            string newMessage;
            if (!State.Player.Bd.BodyParts.ContainsKey("RightHand")) return;
            if (State.Player.Bd.BodyParts["RightHand"].IsUsed)
            {
                State.Player.TryTakeOffItem(State.Player.Bd.BodyParts["RightHand"]);
                newMessage = $"{State.Player.Name} put {State.Player.Eq.Eq[State.Player.Eq.EqPointer].Name} from right hand back to eq";
            }
            else
            {
                if (State.Player.Eq.Eq.Count == 0) return;
                newMessage = $"{State.Player.Name} took {State.Player.Eq.Eq[State.Player.Eq.EqPointer].Name} to right hand";
                State.Player.TryTakeItem(State.Player.Bd, "RightHand");
            }
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class PutInLeftHand(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.L)
        {
            string newMessage;
            if (!State.Player.Bd.BodyParts.ContainsKey("LeftHand")) return;
            if (State.Player.Bd.BodyParts["LeftHand"].IsUsed)
            {
                State.Player.TryTakeOffItem(State.Player.Bd.BodyParts["LeftHand"]);
                newMessage = $"{State.Player.Name} put {State.Player.Eq.Eq[State.Player.Eq.EqPointer].Name} from left hand back to eq";
            }
            else
            {
                if (State.Player.Eq.Eq.Count == 0) return;
                newMessage = $"{State.Player.Name} took {State.Player.Eq.Eq[State.Player.Eq.EqPointer].Name} to left hand";
                State.Player.TryTakeItem(State.Player.Bd, "LeftHand");
            }
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class UseFromLeftHandLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.U && ((int)keyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1)
        {
            string newMessage;
            if (!State.Player.Bd.BodyParts.TryGetValue("LeftHand", out BodyPart? value)) newMessage = "Invalid Input: No LeftHand";
            else if (value.IsUsed)
            {
                newMessage = $"{State.Player.Name} used {value.usedItem!.Name}";
                State.Player.UseItemInHand(value.usedItem!, "LeftHand");
            }
            else
            {
                newMessage = "Invalid Input: No item in LeftHand";
            }
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class UseFromRightHandLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.U && ((int)keyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0)
        {
            string newMessage;
            if (!State.Player.Bd.BodyParts.TryGetValue("RightHand", out BodyPart? value)) newMessage = "Invalid Input: No RightHand";
            else if (value.IsUsed)
            {
                newMessage = $"{State.Player.Name} used {value.usedItem!.Name}";
                State.Player.UseItemInHand(value.usedItem!, "RightHand");
            }
            else
            {
                newMessage = "Invalid Input: No item in RightHand";
            }
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class NormalAttackRightHandLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.Enter && ((int)keyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0)
        {
            string newMessage;
            if (!State.Player.Bd.BodyParts.TryGetValue("RightHand", out BodyPart? value)) newMessage = "Invalid Input: No RightHand";
            else if (value.IsUsed)
            {
                var enemy = State.ChooseEnemyToFight();
                if (enemy == null)
                {
                    newMessage = "No enemy in range";
                }
                else
                {
                    newMessage = $"{State.Player.Name} did a Normal Attack on {enemy.Name} [{enemy.Health}] using {value.usedItem!.Name}";
                    var normalAttack = new NormalPlayerEnemyAttack(state.Player, enemy);
                    value.usedItem.AcceptAttack(normalAttack, null);
                    normalAttack.Attack();
                    normalAttack.CounterAttack();
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in RightHand";
            }
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class NormalAttackLeftHandLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.Enter && ((int)keyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1)
        {
            string newMessage;
            if (!State.Player.Bd.BodyParts.TryGetValue("LeftHand", out BodyPart? value)) newMessage = "Invalid Input: No LeftHand";
            else if (value.IsUsed)
            {
                var enemy = State.ChooseEnemyToFight();
                if (enemy == null)
                {
                    newMessage = "No enemy in range";
                }
                else
                {
                    newMessage = $"{State.Player.Name} did a Normal Attack on {enemy.Name} [{enemy.Health}] using {value.usedItem!.Name}";
                    var normalAttack = new NormalPlayerEnemyAttack(state.Player, enemy);
                    value.usedItem.AcceptAttack(normalAttack, null);
                    normalAttack.Attack();
                    normalAttack.CounterAttack();
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in LeftHand";
            }

            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class SneakAttackRightHandLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.Backspace && ((int)keyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0)
        {
            string newMessage;
            if (!State.Player.Bd.BodyParts.TryGetValue("RightHand", out BodyPart? value)) newMessage = "Invalid Input: No RightHand";
            else if (value.IsUsed)
            {
                var enemy = State.ChooseEnemyToFight();
                if (enemy == null)
                {
                    newMessage = "No enemy in range";
                }
                else
                {
                    newMessage = $"{State.Player.Name} did a Sneak Attack on {enemy.Name} [{enemy.Health}] using {value.usedItem!.Name}";
                    var sneakAttack = new SneakPlayerEnemyAttack(state.Player, enemy);
                    value.usedItem.AcceptAttack(sneakAttack, null);
                    sneakAttack.Attack();
                    sneakAttack.CounterAttack();
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in RightHand";
            }
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class SneakAttackLeftHandLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.Backspace && ((int)keyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1)
        {
            string newMessage;
            if (!State.Player.Bd.BodyParts.TryGetValue("LeftHand", out BodyPart? value)) newMessage = "Invalid Input: No LeftHand";
            else if (value.IsUsed)
            {
                var enemy = State.ChooseEnemyToFight();
                if (enemy == null)
                {
                    newMessage = "No enemy in range";
                }
                else
                {
                    newMessage = $"{State.Player.Name} did a Sneak Attack on {enemy.Name} [{enemy.Health}] using {value.usedItem!.Name}";
                    var sneakAttack = new SneakPlayerEnemyAttack(state.Player, enemy);
                    value.usedItem.AcceptAttack(sneakAttack, null);
                    sneakAttack.Attack();
                    sneakAttack.CounterAttack();
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in LeftHand";
            }
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class MagicAttackRightHandLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.Delete && ((int)keyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0)
        {
            string newMessage;
            if (!State.Player.Bd.BodyParts.TryGetValue("RightHand", out BodyPart? value)) newMessage = "Invalid Input: No RightHand";
            else if (value.IsUsed)
            {
                var enemy = State.ChooseEnemyToFight();
                if (enemy == null)
                {
                    newMessage = "No enemy in range";
                }
                else
                {
                    newMessage = $"{State.Player.Name} did a Magic Attack on {enemy.Name} [{enemy.Health}] using {value.usedItem!.Name}";
                    var magicAttack = new MagicPlayerEnemyAttack(state.Player, enemy);
                    value.usedItem.AcceptAttack(magicAttack, null);
                    magicAttack.Attack();
                    magicAttack.CounterAttack();
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in RightHand";
            }
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class MagicAttackLeftHandLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.Delete && ((int)keyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1)
        {
            string newMessage;
            if (!State.Player.Bd.BodyParts.TryGetValue("LeftHand", out BodyPart? value)) newMessage = "Invalid Input: No LeftHand";
            else if (value.IsUsed)
            {
                var enemy = State.ChooseEnemyToFight();
                if (enemy == null)
                {
                    newMessage = "No enemy in range";
                }
                else
                {
                    newMessage = $"{State.Player.Name} did a Magic Attack on {enemy.Name} [{enemy.Health}] using {value.usedItem!.Name}";
                    var magicAttack = new MagicPlayerEnemyAttack(state.Player, enemy);
                    value.usedItem.AcceptAttack(magicAttack, null);
                    magicAttack.Attack();
                    magicAttack.CounterAttack();
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in LeftHand";
            }
            
            Logs.AddLogMessage(newMessage);
            var logDisp = Display.Display.GetInstance();
            logDisp.DisplayLog(Logs);
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class ExitLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.Escape)
        {
            State.QuitGame();
        }
        else
        {
            NextLink.HandleInput(keyInfo);
        }
    }
}

public class SentinelLink(GameState state, Logs logs)
    : ConsoleInputHandlerLink(state, logs)
{
    public override void HandleInput(ConsoleKeyInfo keyInfo)
    {
        Logs.AddLogMessage("Bad input or an unassociated key pressed.");
        var logDisp = Display.Display.GetInstance();
        logDisp.DisplayLog(Logs);
    }
}
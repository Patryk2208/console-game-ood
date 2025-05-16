using RPG_ood.Commands;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game.Attack;
using RPG_ood.Model.Game.GameState;

namespace RPG_ood.Input;

public abstract class ConsoleInputHandlerLink (GameState state)
{
    protected ConsoleInputHandlerLink NextLink { get; set; }
    protected GameState State { get; } = state;

    public void SetNextLink(ConsoleInputHandlerLink nextLink)
    {
        NextLink = nextLink;
    }
    public abstract void HandleInput(Command command);
}

public class VerifyUserLink(GameState state) : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (State.Players.ContainsKey(command.PlayerId) && !State.Players[command.PlayerId].IsDead)
        {
            NextLink.HandleInput(command);
        }
        else
        {
            State.Logs.AddLogMessage("Player with specified id does not exist or is dead");
        }
    }
}

public class MoveUpLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.W)
        {
            var p = State.Players[command.PlayerId];
            p.MoveUp(State.CurrentRoom);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class MoveDownLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.S)
        {
            var p = State.Players[command.PlayerId];
            p.MoveDown(State.CurrentRoom);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class MoveLeftLink(GameState state) 
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.A)
        {
            var p = State.Players[command.PlayerId];
            p.MoveLeft(State.CurrentRoom);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class MoveRightLink(GameState state) 
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.D)
        {
            var p = State.Players[command.PlayerId];
            p.MoveRight(State.CurrentRoom);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class EquipLink(GameState state) 
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.E)
        {
            var p = State.Players[command.PlayerId];
            var itemsAtPos = State.CurrentRoom.GetItemsAtPos(p.Pos).ToList();
            if (!itemsAtPos.Any()) return;
            var item = itemsAtPos.ElementAt(p.PickUpCursor);
            var newMessage = $"{p.Name} picked up {item.Name} at {item.Pos}";
            item.Pos = item.Pos with { X = -1, Y = -1 };
            p.PickUpItem(item);
            
            State.Logs.AddLogMessage(newMessage);
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class ThrowLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.T)
        {
            var p = State.Players[command.PlayerId];
            if (p.Eq.Eq.Count <= 0) return;
            var newMessage =
                $"{p.Name} dropped {p.Eq.Eq[p.Eq.EqPointer].Name}";
            p.DropItem();
            
            State.Logs.AddLogMessage(newMessage);
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class ThrowAllLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.Y)
        {
            var p = State.Players[command.PlayerId];
            if (p.Eq.Eq.Count <= 0) return;
            var newMessage = $"{p.Name} dropped all items";
            while (p.Eq.Eq.Count > 0)
            {
                p.DropItem();
            }
            
            State.Logs.AddLogMessage(newMessage);
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class EqSelectDownLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.LeftArrow)
        {
            var p = State.Players[command.PlayerId];
            p.Eq.TryMovePointerLeft();
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class EqSelectUpLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.RightArrow)
        {
            var p = State.Players[command.PlayerId];
            p.Eq.TryMovePointerRight();
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class PickUpSelectDownLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.DownArrow)
        {
            var p = State.Players[command.PlayerId];
            p.PickUpCursor++;
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class PickUpSelectUpLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.UpArrow)
        {
            var p = State.Players[command.PlayerId];
            p.PickUpCursor = Math.Max(0, p.PickUpCursor - 1);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class PutInRightHand(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.P)
        {
            var p = State.Players[command.PlayerId];
            string newMessage;
            if (!p.Bd.BodyParts.ContainsKey("RightHand")) return;
            if (p.Bd.BodyParts["RightHand"].IsUsed)
            {
                p.TryTakeOffItem(p.Bd.BodyParts["RightHand"]);
                newMessage = $"{p.Name} put {p.Eq.Eq[p.Eq.EqPointer].Name} from right hand back to eq";
            }
            else
            {
                if (p.Eq.Eq.Count == 0) return;
                newMessage = $"{p.Name} took {p.Eq.Eq[p.Eq.EqPointer].Name} to right hand";
                p.TryTakeItem(p.Bd, "RightHand");
            }

            State.Logs.AddLogMessage(newMessage);
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class PutInLeftHand(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.L)
        {
            var p = State.Players[command.PlayerId];
            string newMessage;
            if (!p.Bd.BodyParts.ContainsKey("LeftHand")) return;
            if (p.Bd.BodyParts["LeftHand"].IsUsed)
            {
                p.TryTakeOffItem(p.Bd.BodyParts["LeftHand"]);
                newMessage = $"{p.Name} put {p.Eq.Eq[p.Eq.EqPointer].Name} from left hand back to eq";
            }
            else
            {
                if (p.Eq.Eq.Count == 0) return;
                newMessage = $"{p.Name} took {p.Eq.Eq[p.Eq.EqPointer].Name} to left hand";
                p.TryTakeItem(p.Bd, "LeftHand");
            }
            
            State.Logs.AddLogMessage(newMessage);
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class UseFromLeftHandLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.U && ((int)command.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1)
        {
            var p = State.Players[command.PlayerId];
            string newMessage;
            if (!p.Bd.BodyParts.TryGetValue("LeftHand", out BodyPart? value)) newMessage = "Invalid Input: No LeftHand";
            else if (value.IsUsed)
            {
                newMessage = $"{p.Name} used {value.UsedItem!.Name}";
                p.UseItemInHand(value.UsedItem!, "LeftHand");
            }
            else
            {
                newMessage = "Invalid Input: No item in LeftHand";
            }
            
            State.Logs.AddLogMessage(newMessage);
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class UseFromRightHandLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.U && ((int)command.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0)
        {
            var p = State.Players[command.PlayerId];
            string newMessage;
            if (!p.Bd.BodyParts.TryGetValue("RightHand", out BodyPart? value)) newMessage = "Invalid Input: No RightHand";
            else if (value.IsUsed)
            {
                newMessage = $"{p.Name} used {value.UsedItem!.Name}";
                p.UseItemInHand(value.UsedItem!, "RightHand");
            }
            else
            {
                newMessage = "Invalid Input: No item in RightHand";
            }
            
            State.Logs.AddLogMessage(newMessage);
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class NormalAttackRightHandLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.Enter && ((int)command.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0
            || command.KeyInfo.Key == ConsoleKey.D1)
        {
            var p = State.Players[command.PlayerId];
            var newMessage = "";
            if (!p.Bd.BodyParts.TryGetValue("RightHand", out BodyPart? value)) newMessage = "Invalid Input: No RightHand";
            else if (value.IsUsed)
            {
                var rivalPlayer = State.ChoosePlayerToFight(p);
                if (rivalPlayer != null)
                {
                    newMessage =
                        $"{p.Name} did a Normal Attack on {rivalPlayer.Name}";
                    Fight normalAttack = new NormalPlayersAttack(p, rivalPlayer, value.UsedItem!);
                    value.UsedItem!.AcceptAttack(normalAttack);
                    normalAttack.Attack();
                    normalAttack.CounterAttack();
                }
                else
                {
                    var enemy = State.ChooseEnemyToFight(p);
                    if (enemy == null)
                    {
                        newMessage = "Nobody in range";
                    }
                    else
                    {
                        newMessage =
                            $"{p.Name} did a Normal Attack on {enemy.Name} [{enemy.Health}] using {value.UsedItem!.Name}";
                        Fight normalAttack = new NormalPlayerEnemyAttack(p, enemy, value.UsedItem!);
                        value.UsedItem!.AcceptAttack(normalAttack);
                        normalAttack.Attack();
                        normalAttack.CounterAttack();
                    }
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in RightHand";
            }
            
            State.Logs.AddLogMessage(newMessage);
            
            if (command.KeyInfo.Key == ConsoleKey.D1 && (value == null || value!.UsedItem == null || !value.UsedItem.IsTwoHanded))
            {
                NextLink.HandleInput(command);
            }
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class NormalAttackLeftHandLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.Enter && ((int)command.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1
            || command.KeyInfo.Key == ConsoleKey.D1)
        {
            var p = State.Players[command.PlayerId];
            string newMessage;
            if (!p.Bd.BodyParts.TryGetValue("LeftHand", out BodyPart? value)) newMessage = "Invalid Input: No LeftHand";
            else if (value.IsUsed)
            {
                var rivalPlayer = State.ChoosePlayerToFight(p);
                if (rivalPlayer != null)
                {
                    newMessage =
                        $"{p.Name} did a Normal Attack on {rivalPlayer.Name}";
                    Fight normalAttack = new NormalPlayersAttack(p, rivalPlayer, value.UsedItem!);
                    value.UsedItem!.AcceptAttack(normalAttack);
                    normalAttack.Attack();
                    normalAttack.CounterAttack();
                }
                else
                {
                    var enemy = State.ChooseEnemyToFight(p);
                    if (enemy == null)
                    {
                        newMessage = "Nobody in range";
                    }
                    else
                    {
                        newMessage =
                            $"{p.Name} did a Normal Attack on {enemy.Name} [{enemy.Health}] using {value.UsedItem!.Name}";
                        Fight normalAttack = new NormalPlayerEnemyAttack(p, enemy, value.UsedItem!);
                        value.UsedItem!.AcceptAttack(normalAttack);
                        normalAttack.Attack();
                        normalAttack.CounterAttack();
                    }
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in LeftHand";
            }

            
            State.Logs.AddLogMessage(newMessage);
            
            if (command.KeyInfo.Key == ConsoleKey.D1 && (value == null || value!.UsedItem == null || !value.UsedItem.IsTwoHanded))
            {
                NextLink.HandleInput(command);
            }
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class SneakAttackRightHandLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.Backspace && ((int)command.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0
            || command.KeyInfo.Key == ConsoleKey.D2)
        {
            var p = State.Players[command.PlayerId];
            string newMessage;
            if (!p.Bd.BodyParts.TryGetValue("RightHand", out BodyPart? value)) newMessage = "Invalid Input: No RightHand";
            else if (value.IsUsed)
            {
                var rivalPlayer = State.ChoosePlayerToFight(p);
                if (rivalPlayer != null)
                {
                    newMessage =
                        $"{p.Name} did a Sneak Attack on {rivalPlayer.Name}";
                    Fight sneakAttack = new SneakPlayersAttack(p, rivalPlayer, value.UsedItem!);
                    value.UsedItem!.AcceptAttack(sneakAttack);
                    sneakAttack.Attack();
                    sneakAttack.CounterAttack();
                }
                else
                {
                    var enemy = State.ChooseEnemyToFight(p);
                    if (enemy == null)
                    {
                        newMessage = "Nobody in range";
                    }
                    else
                    {
                        newMessage =
                            $"{p.Name} did a Sneak Attack on {enemy.Name} [{enemy.Health}] using {value.UsedItem!.Name}";
                        Fight sneakAttack = new SneakPlayerEnemyAttack(p, enemy, value.UsedItem!);
                        value.UsedItem!.AcceptAttack(sneakAttack);
                        sneakAttack.Attack();
                        sneakAttack.CounterAttack();
                    }
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in RightHand";
            }
            
            State.Logs.AddLogMessage(newMessage);
            
            if (command.KeyInfo.Key == ConsoleKey.D2 && (value == null || value!.UsedItem == null || !value.UsedItem.IsTwoHanded))
            {
                NextLink.HandleInput(command);
            }
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class SneakAttackLeftHandLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.Backspace && ((int)command.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1
            || command.KeyInfo.Key == ConsoleKey.D2)
        {
            var p = State.Players[command.PlayerId];
            string newMessage;
            if (!p.Bd.BodyParts.TryGetValue("LeftHand", out BodyPart? value)) newMessage = "Invalid Input: No LeftHand";
            else if (value.IsUsed)
            {
                var rivalPlayer = State.ChoosePlayerToFight(p);
                if (rivalPlayer != null)
                {
                    newMessage =
                        $"{p.Name} did a Sneak Attack on {rivalPlayer.Name}";
                    Fight sneakAttack = new SneakPlayersAttack(p, rivalPlayer, value.UsedItem!);
                    value.UsedItem!.AcceptAttack(sneakAttack);
                    sneakAttack.Attack();
                    sneakAttack.CounterAttack();
                }
                else
                {
                    var enemy = State.ChooseEnemyToFight(p);
                    if (enemy == null)
                    {
                        newMessage = "Nobody in range";
                    }
                    else
                    {
                        newMessage =
                            $"{p.Name} did a Sneak Attack on {enemy.Name} [{enemy.Health}] using {value.UsedItem!.Name}";
                        Fight sneakAttack = new SneakPlayerEnemyAttack(p, enemy, value.UsedItem!);
                        value.UsedItem!.AcceptAttack(sneakAttack);
                        sneakAttack.Attack();
                        sneakAttack.CounterAttack();
                    }
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in LeftHand";
            }
            
            State.Logs.AddLogMessage(newMessage);
            
            if (command.KeyInfo.Key == ConsoleKey.D2 && (value == null || value!.UsedItem == null || !value.UsedItem.IsTwoHanded))
            {
                NextLink.HandleInput(command);
            }
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class MagicAttackRightHandLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.Delete && ((int)command.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 0
            || command.KeyInfo.Key == ConsoleKey.D3)
        {
            var p = State.Players[command.PlayerId];
            string newMessage;
            if (!p.Bd.BodyParts.TryGetValue("RightHand", out BodyPart? value)) newMessage = "Invalid Input: No RightHand";
            else if (value.IsUsed)
            {
                var rivalPlayer = State.ChoosePlayerToFight(p);
                if (rivalPlayer != null)
                {
                    newMessage =
                        $"{p.Name} did a Magic Attack on {rivalPlayer.Name}";
                    Fight magicAttack = new MagicPlayersAttack(p, rivalPlayer, value.UsedItem!);
                    value.UsedItem!.AcceptAttack(magicAttack);
                    magicAttack.Attack();
                    magicAttack.CounterAttack();
                }
                else
                {
                    var enemy = State.ChooseEnemyToFight(p);
                    if (enemy == null)
                    {
                        newMessage = "Nobody in range";
                    }
                    else
                    {
                        newMessage =
                            $"{p.Name} did a Sneak Attack on {enemy.Name} [{enemy.Health}] using {value.UsedItem!.Name}";
                        Fight magicAttack = new MagicPlayerEnemyAttack(p, enemy, value.UsedItem!);
                        value.UsedItem!.AcceptAttack(magicAttack);
                        magicAttack.Attack();
                        magicAttack.CounterAttack();
                    }
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in RightHand";
            }
            
            State.Logs.AddLogMessage(newMessage);
            
            if (command.KeyInfo.Key == ConsoleKey.D3 && (value == null || value!.UsedItem == null || !value.UsedItem.IsTwoHanded))
            {
                NextLink.HandleInput(command);
            }
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class MagicAttackLeftHandLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.Delete && ((int)command.KeyInfo.Modifiers & (int)ConsoleModifiers.Alt) == 1
            || command.KeyInfo.Key == ConsoleKey.D3)
        {
            var p = State.Players[command.PlayerId];
            string newMessage;
            if (!p.Bd.BodyParts.TryGetValue("LeftHand", out BodyPart? value)) newMessage = "Invalid Input: No LeftHand";
            else if (value.IsUsed)
            {
                var rivalPlayer = State.ChoosePlayerToFight(p);
                if (rivalPlayer != null)
                {
                    newMessage =
                        $"{p.Name} did a Magic Attack on {rivalPlayer.Name}";
                    Fight magicAttack = new MagicPlayersAttack(p, rivalPlayer, value.UsedItem!);
                    value.UsedItem!.AcceptAttack(magicAttack);
                    magicAttack.Attack();
                    magicAttack.CounterAttack();
                }
                else
                {
                    var enemy = State.ChooseEnemyToFight(p);
                    if (enemy == null)
                    {
                        newMessage = "Nobody in range";
                    }
                    else
                    {
                        newMessage =
                            $"{p.Name} did a Sneak Attack on {enemy.Name} [{enemy.Health}] using {value.UsedItem!.Name}";
                        Fight magicAttack = new MagicPlayerEnemyAttack(p, enemy, value.UsedItem!);
                        value.UsedItem!.AcceptAttack(magicAttack);
                        magicAttack.Attack();
                        magicAttack.CounterAttack();
                    }
                }
            }
            else
            {
                newMessage = "Invalid Input: No item in LeftHand";
            }
            
            State.Logs.AddLogMessage(newMessage);
        
            if (command.KeyInfo.Key == ConsoleKey.D3 && (value == null || value!.UsedItem == null || !value.UsedItem.IsTwoHanded))
            {
                NextLink.HandleInput(command);
            }
            //Display.GetInstance().DisplayLog(State.Logs);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class ExitLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        if (command.KeyInfo.Key == ConsoleKey.Escape)
        {
            State.RemovePlayer(command.PlayerId);
        }
        else
        {
            NextLink.HandleInput(command);
        }
    }
}

public class SentinelLink(GameState state)
    : ConsoleInputHandlerLink(state)
{
    public override void HandleInput(Command command)
    {
        State.Logs.AddLogMessage("Bad input or an unassociated key pressed.");
        //Display.GetInstance().DisplayLog(State.Logs);
    }
}
using System.Text.Json.Serialization;
using Model.Game.Attack;
using Model.Game.Beings;
using Model.Game.GameState;

namespace Model.Commands;

[Serializable]
[JsonPolymorphic]
//[JsonConverter(typeof(CommandJsonConverter))]
[JsonDerivedType(typeof(MoveUpCommand), nameof(MoveUpCommand))]
[JsonDerivedType(typeof(MoveDownCommand), nameof(MoveDownCommand))]
[JsonDerivedType(typeof(MoveRightCommand), nameof(MoveRightCommand))]
[JsonDerivedType(typeof(MoveLeftCommand), nameof(MoveLeftCommand))]
[JsonDerivedType(typeof(EquipCommand), nameof(EquipCommand))]
[JsonDerivedType(typeof(ThrowCommand), nameof(ThrowCommand))]
[JsonDerivedType(typeof(ThrowAllCommand), nameof(ThrowAllCommand))]
[JsonDerivedType(typeof(EqSelectDownCommand), nameof(EqSelectDownCommand))]
[JsonDerivedType(typeof(EqSelectUpCommand), nameof(EqSelectUpCommand))]
[JsonDerivedType(typeof(PickUpSelectDownCommand), nameof(PickUpSelectDownCommand))]
[JsonDerivedType(typeof(PickUpSelectUpCommand), nameof(PickUpSelectUpCommand))]
[JsonDerivedType(typeof(PutInLeftHandCommand), nameof(PutInLeftHandCommand))]
[JsonDerivedType(typeof(PutInRightHandCommand), nameof(PutInRightHandCommand))]
[JsonDerivedType(typeof(UseFromLeftHandCommand), nameof(UseFromLeftHandCommand))]
[JsonDerivedType(typeof(UseFromRightHandCommand), nameof(UseFromRightHandCommand))]
[JsonDerivedType(typeof(NormalAttackLeftHandCommand), nameof(NormalAttackRightHandCommand))]
[JsonDerivedType(typeof(NormalAttackRightHandCommand), nameof(NormalAttackLeftHandCommand))]
[JsonDerivedType(typeof(SneakAttackLeftHandCommand), nameof(SneakAttackLeftHandCommand))]
[JsonDerivedType(typeof(SneakAttackRightHandCommand), nameof(SneakAttackRightHandCommand))]
[JsonDerivedType(typeof(MagicAttackLeftHandCommand), nameof(MagicAttackLeftHandCommand))]
[JsonDerivedType(typeof(MagicAttackRightHandCommand), nameof(MagicAttackRightHandCommand))]
[JsonDerivedType(typeof(ExitCommand), nameof(ExitCommand))]
public abstract class Command
{
    public long PlayerId { get; set; }

    [JsonConstructor]
    protected Command(long playerId)
    {
        PlayerId = playerId;
    }
    public abstract void Execute(GameState state);
}

/*public class CommandJsonConverter : JsonConverter<Command>
{
    public override Command? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            var keyInfo = root.GetProperty("KeyInfo");
            var keyChar = keyInfo.GetProperty("KeyChar").Deserialize<char>();
            var consoleKey = Enum.Parse<ConsoleKey>(keyInfo.GetProperty("ConsoleKey").GetString());
            var mods = Enum.Parse<ConsoleModifiers>(keyInfo.GetProperty("Modifiers").ToString());
            var playerId = root.GetProperty("PlayerId").GetInt64();
            return new Command(
                new ConsoleKeyInfo(keyChar, consoleKey, 
                    mods == ConsoleModifiers.Shift, 
                    mods == ConsoleModifiers.Alt, 
                    mods == ConsoleModifiers.Control),
                playerId);
        }
    }

    public override void Write(Utf8JsonWriter writer, Command value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("KeyInfo");
        writer.WriteStartObject();
        writer.WritePropertyName("KeyChar");
        writer.WriteStringValue(value.KeyInfo.KeyChar.ToString());
        writer.WritePropertyName("ConsoleKey");
        writer.WriteStringValue(value.KeyInfo.Key.ToString());
        writer.WritePropertyName("Modifiers");
        writer.WriteStringValue(value.KeyInfo.Modifiers .ToString());
        writer.WriteEndObject();
        writer.WritePropertyName("PlayerId");
        writer.WriteNumberValue(value.PlayerId);
        writer.WriteEndObject();
    }
}*/

public class MoveUpCommand : Command
{
    [JsonConstructor]
    public MoveUpCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        p.MoveUp(state.CurrentRoom);
    }
}

public class MoveDownCommand : Command
{
    [JsonConstructor]
    public MoveDownCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        p.MoveDown(state.CurrentRoom);
    }
}

public class MoveLeftCommand : Command
{
    [JsonConstructor]
    public MoveLeftCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        p.MoveLeft(state.CurrentRoom);
    }
}

public class MoveRightCommand : Command
{
    [JsonConstructor]
    public MoveRightCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        p.MoveRight(state.CurrentRoom);
    }
}

public class EquipCommand : Command
{
    [JsonConstructor]
    public EquipCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        var itemsAtPos = state.CurrentRoom.GetItemsAtPos(p.Pos).ToList();
        if (!itemsAtPos.Any()) return;
        var item = itemsAtPos.ElementAt(p.PickUpCursor);
        var newMessage = $"{p.Name} picked up {item.Name} at {item.Pos}";
        item.Pos = item.Pos with { X = -1, Y = -1 };
        p.PickUpItem(item);
            
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class ThrowCommand : Command
{
    [JsonConstructor]
    public ThrowCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        if (p.Eq.Eq.Count <= 0) return;
        var newMessage = $"{p.Name} dropped {p.Eq.Eq[p.Eq.EqPointer].Name}";
        p.DropItem();
            
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class ThrowAllCommand : Command
{
    [JsonConstructor]
    public ThrowAllCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        if (p.Eq.Eq.Count <= 0) return;
        var newMessage = $"{p.Name} dropped all items";
        while (p.Eq.Eq.Count > 0)
        {
            p.DropItem();
        }
            
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class EqSelectDownCommand : Command
{
    [JsonConstructor]
    public EqSelectDownCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        p.Eq.TryMovePointerLeft();
    }
}

public class EqSelectUpCommand : Command
{
    [JsonConstructor]
    public EqSelectUpCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        p.Eq.TryMovePointerRight();
    }
}

public class PickUpSelectDownCommand : Command
{
    [JsonConstructor]
    public PickUpSelectDownCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        p.PickUpCursor++;
    }
}

public class PickUpSelectUpCommand : Command
{
    [JsonConstructor]
    public PickUpSelectUpCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        p.PickUpCursor = Math.Max(0, p.PickUpCursor - 1);
    }
}

public class PutInRightHandCommand : Command
{
    [JsonConstructor]
    public PutInRightHandCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
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

        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class PutInLeftHandCommand : Command
{
    [JsonConstructor]
    public PutInLeftHandCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
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
            
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class UseFromLeftHandCommand : Command
{
    [JsonConstructor]
    public UseFromLeftHandCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
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
            
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class UseFromRightHandCommand : Command
{
    [JsonConstructor]
    public UseFromRightHandCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
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
            
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class NormalAttackRightHandCommand : Command
{
    [JsonConstructor]
    public NormalAttackRightHandCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        var newMessage = "";
        if (!p.Bd.BodyParts.TryGetValue("RightHand", out BodyPart? value)) newMessage = "Invalid Input: No RightHand";
        else if (value.IsUsed)
        {
            var rivalPlayer = state.ChoosePlayerToFight(p);
            if (rivalPlayer != null)
            {
                newMessage =
                    $"{p.Name} did a Normal Attack on {rivalPlayer.Name}";
                Fight normalAttack = new NormalPlayersAttack(p, rivalPlayer, value.UsedItem!);
                value.UsedItem!.AcceptAttack(normalAttack);
                normalAttack.Attack();
                state.Logs.AddCommonLogMessage(newMessage);
                return;
            }
            else
            {
                var enemy = state.ChooseEnemyToFight(p);
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
                }
            }
        }
        else
        {
            newMessage = "Invalid Input: No item in RightHand";
        }
           
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class NormalAttackLeftHandCommand : Command
{
    [JsonConstructor]
    public NormalAttackLeftHandCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        string newMessage;
        if (!p.Bd.BodyParts.TryGetValue("LeftHand", out BodyPart? value)) newMessage = "Invalid Input: No LeftHand";
        else if (value.IsUsed)
        {
            var rivalPlayer = state.ChoosePlayerToFight(p);
            if (rivalPlayer != null)
            {
                newMessage =
                    $"{p.Name} did a Normal Attack on {rivalPlayer.Name}";
                Fight normalAttack = new NormalPlayersAttack(p, rivalPlayer, value.UsedItem!);
                value.UsedItem!.AcceptAttack(normalAttack);
                normalAttack.Attack();
                state.Logs.AddCommonLogMessage(newMessage);
                return;
            }
            else
            {
                var enemy = state.ChooseEnemyToFight(p);
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
                }
            }
        }
        else
        {
            newMessage = "Invalid Input: No item in LeftHand";
        }

            
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class SneakAttackRightHandCommand : Command
{
    [JsonConstructor]
    public SneakAttackRightHandCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        string newMessage;
        if (!p.Bd.BodyParts.TryGetValue("RightHand", out BodyPart? value)) newMessage = "Invalid Input: No RightHand";
        else if (value.IsUsed)
        {
            var rivalPlayer = state.ChoosePlayerToFight(p);
            if (rivalPlayer != null)
            {
                newMessage =
                    $"{p.Name} did a Sneak Attack on {rivalPlayer.Name}";
                Fight sneakAttack = new SneakPlayersAttack(p, rivalPlayer, value.UsedItem!);
                value.UsedItem!.AcceptAttack(sneakAttack);
                sneakAttack.Attack();
                state.Logs.AddCommonLogMessage(newMessage);
                return;
            }
            else
            {
                var enemy = state.ChooseEnemyToFight(p);
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
                }
            }
        }
        else
        {
            newMessage = "Invalid Input: No item in RightHand";
        }
            
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class SneakAttackLeftHandCommand : Command
{
    [JsonConstructor]
    public SneakAttackLeftHandCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        string newMessage;
        if (!p.Bd.BodyParts.TryGetValue("LeftHand", out BodyPart? value)) newMessage = "Invalid Input: No LeftHand";
        else if (value.IsUsed)
        {
            var rivalPlayer = state.ChoosePlayerToFight(p);
            if (rivalPlayer != null)
            {
                newMessage =
                    $"{p.Name} did a Sneak Attack on {rivalPlayer.Name}";
                Fight sneakAttack = new SneakPlayersAttack(p, rivalPlayer, value.UsedItem!);
                value.UsedItem!.AcceptAttack(sneakAttack);
                sneakAttack.Attack();
                state.Logs.AddCommonLogMessage(newMessage);
                return;
            }
            else
            {
                var enemy = state.ChooseEnemyToFight(p);
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
                }
            }
        }
        else
        {
            newMessage = "Invalid Input: No item in LeftHand";
        }
            
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class MagicAttackRightHandCommand : Command
{
    [JsonConstructor]
    public MagicAttackRightHandCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        string newMessage;
        if (!p.Bd.BodyParts.TryGetValue("RightHand", out BodyPart? value)) newMessage = "Invalid Input: No RightHand";
        else if (value.IsUsed)
        {
            var rivalPlayer = state.ChoosePlayerToFight(p);
            if (rivalPlayer != null)
            {
                newMessage =
                    $"{p.Name} did a Magic Attack on {rivalPlayer.Name}";
                Fight magicAttack = new MagicPlayersAttack(p, rivalPlayer, value.UsedItem!);
                value.UsedItem!.AcceptAttack(magicAttack);
                magicAttack.Attack();
                state.Logs.AddCommonLogMessage(newMessage);
                return;
            }
            else
            {
                var enemy = state.ChooseEnemyToFight(p);
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
                }
            }
        }
        else
        {
            newMessage = "Invalid Input: No item in RightHand";
        }
            
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class MagicAttackLeftHandCommand : Command
{
    [JsonConstructor]
    public MagicAttackLeftHandCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        var p = state.Players[PlayerId];
        string newMessage;
        if (!p.Bd.BodyParts.TryGetValue("LeftHand", out BodyPart? value)) newMessage = "Invalid Input: No LeftHand";
        else if (value.IsUsed)
        {
            var rivalPlayer = state.ChoosePlayerToFight(p);
            if (rivalPlayer != null)
            {
                newMessage =
                    $"{p.Name} did a Magic Attack on {rivalPlayer.Name}";
                Fight magicAttack = new MagicPlayersAttack(p, rivalPlayer, value.UsedItem!);
                value.UsedItem!.AcceptAttack(magicAttack);
                magicAttack.Attack();
                state.Logs.AddCommonLogMessage(newMessage);
                return;
            }
            else
            {
                var enemy = state.ChooseEnemyToFight(p);
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
                }
            }
        }
        else
        {
            newMessage = "Invalid Input: No item in LeftHand";
        }
            
        state.Logs.AddLogMessage(PlayerId, newMessage);
    }
}

public class ExitCommand : Command
{
    [JsonConstructor]
    public ExitCommand(long playerId) : base(playerId) {}
    public override void Execute(GameState state)
    {
        state.RemovePlayer(PlayerId);
    }
}

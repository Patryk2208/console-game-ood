using System.Text.Json;
using System.Text.Json.Serialization;
using RPG_ood.Map;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game.Attributes;
using RPG_ood.Model.Game.GameState;
using RPG_ood.Model.Game.Items;
using RPG_ood.Model.GameSnapshot;
using RPG_ood.View;
using Attributes_Attribute = RPG_ood.Model.Game.Attributes.Attribute;


namespace RPG_ood.Model.Game.Beings;

[Serializable]
[JsonConverter(typeof(PlayerJsonConverter))]
public class Player : IBeing
{
    public long Id { get; set; }
    public string Name { get; init; }
    public bool IsDead { get; set; }
    public bool WasAttacked { get; set; }
    public Position Pos { get; set; }
    public Body Bd { get; init; }
    public Dictionary<string, Attributes_Attribute> Attr { get; protected set; }
    public Equipment Eq { get; protected set; }
    public int PickUpCursor { get; set; } = 0;
    [JsonIgnore]
    public MomentChangedEvent MomentChangedEvent { get; init; }

    public Player(string name)
    {
        Name = name;
        Pos = new Position(0, 0);
        MomentChangedEvent = new MomentChangedEvent();

        Attr = new Dictionary<string, Attributes_Attribute>();
        var p = new Power();
        var a = new Agility();
        var h = new Health();
        var l = new Luck();
        var ag = new Aggression();
        var w = new Wisdom();
        Attr.Add(p.Name, p);
        Attr.Add(a.Name, a);
        Attr.Add(h.Name, h);
        Attr.Add(l.Name, l);
        Attr.Add(ag.Name, ag);
        Attr.Add(w.Name, w);

        Eq = new Equipment();

        Bd = new Body();
        Bd.AddBodyPart(new LeftHand());
        Bd.AddBodyPart(new RightHand());
    }

    [JsonConstructor]
    public Player(string name, bool isDead, bool wasAttacked, Position pos, Body bd, 
        Dictionary<string, Attributes_Attribute> attr, Equipment eq, int pickUpCursor)
    {
        Name = name;
        IsDead = isDead;
        WasAttacked = wasAttacked;
        Pos = pos;
        Bd = bd;
        Attr = attr;
        Eq = eq;
        PickUpCursor = pickUpCursor;
    }
    void IObserver.Update(GameState.GameState? state, long id)
    {
        if (state == null) return;
        if (IsDead)
        {
            state.MomentChangedEvent.RemoveObserver(Id.ToString(), this);
            state.CurrentRoom.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = -1, Y = -1 };
            return;
        }
        WasAttacked = false;
        MomentChangedEvent.NotifyObservers(state, Id);
    }
    public void UseItemInHand(IUsable item, string bpName)
    {
        item.Use(this, bpName);
    }
    public void PickUpItem(IItem item)
    {
        item.Interact(this);
    }

    public void TryTakeItem(Body b, string bpName)
    {
        var item = Eq.Eq[Eq.EqPointer];
        if (item.Apply(b, bpName))
        {
            Eq.RemoveItemFromEq();
        }
        
    }
    public void TryTakeOffItem(BodyPart b)
    {
        var item = b.UsedItem!;
        foreach (var bp in Bd.BodyParts.Values)
        {
            if (bp.UsedItem == item)
            {
                bp.TakeOff();
            }
        }
        PickUpItem(item);
    }

    public void DropItem()
    {
        var item = Eq.RemoveItemFromEq();
        item.Pos = Pos;
    }

    public Dictionary<string, int> GetAttributes()
    {
        var res = Attr.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value);
        var manip = Bd.BodyParts.Values
            .Where(i => i.IsUsed)
            .Distinct()
            .Select(i => i.UsedItem!);
        foreach (var it in manip)
        {
            it.AssignAttributes(res);
        }
        return res;
    }

    public List<IItem> GetItemsAtPosFromRoom(RelativeRoomState relativeRoom)
    {
        var itemsAtPos = relativeRoom.GetItemsAtPos(Pos).ToList();
        if (itemsAtPos.Count > 0 && itemsAtPos.Count - 1 < PickUpCursor)
        {
            PickUpCursor = itemsAtPos.Count - 1;
        }
        return itemsAtPos;
    }
    
    public void MoveUp(Room room)
    {
        if (Pos.X - 1 >= 0 && room.Elements[Pos.X - 1, Pos.Y].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X - 1, Y = Pos.Y };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Up";
        }
    }
    
    public void MoveDown(Room room)
    {
        if (Pos.X + 1 < room.Height && room.Elements[Pos.X + 1, Pos.Y].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X + 1, Y = Pos.Y };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Down";
        }
    }
    
    public void MoveLeft(Room room)
    {
        if (Pos.Y - 1 >= 0 && room.Elements[Pos.X, Pos.Y - 1].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X, Y = Pos.Y - 1 };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Left";
        }
    }
    
    public void MoveRight(Room room)
    {
        if (Pos.Y + 1 < room.Width && room.Elements[Pos.X, Pos.Y + 1].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X, Y = Pos.Y + 1 };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Right";
        }
    }
    public void ReceiveDamage(int damage)
    {
        Attr["Health"].Value -= damage;
        if (damage > 0)
        {
            WasAttacked = true;
        }
        if (Attr["Health"].Value <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        IsDead = true;
    }

    public IEnemy? CanFight()
    {
        return null;
    }

    public void AcceptView(IViewGenerator generator)
    {
        generator.VisitPlayer(this);
    }
}

public class PlayerJsonConverter : JsonConverter<Player>
{
    public override Player? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            var name = root.GetProperty("Name").GetString();
            var isDead = root.GetProperty("IsDead").GetBoolean();
            var wasAttacked = root.GetProperty("WasAttacked").GetBoolean();
            var pos = root.GetProperty("Pos").Deserialize<Position>();
            var bdElem = root.GetProperty("Bd");
            //var bdParts = bdElem.GetProperty("BodyParts").Deserialize<Dictionary<string, BodyPart>>();
            var bdParts = new Dictionary<string, BodyPart>();
            var bdps = bdElem.GetProperty("BodyParts");
            foreach (var bdp in bdps.EnumerateObject())
            {
                bdParts[bdp.Name] = bdp.Value.Deserialize<BodyPart>()!;
                bdParts[bdp.Name].PutOn(bdp.Value.GetProperty("UsedItem").Deserialize<IUsable>()!);
            }
            var attr = root.GetProperty("Attr").Deserialize<Dictionary<string, Attributes_Attribute>>();
            var eq = root.GetProperty("Eq").Deserialize<Equipment>();
            var puc = root.GetProperty("PickUpCursor").GetInt32();
            return new Player(name, isDead, wasAttacked, pos, new Body(bdParts), attr, eq, puc);
        }
    }

    public override void Write(Utf8JsonWriter writer, Player value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Name", value.Name);
        writer.WriteBoolean("IsDead", value.IsDead);
        writer.WriteBoolean("WasAttacked", value.WasAttacked);
        writer.WritePropertyName("Pos");
        JsonSerializer.Serialize<Position>(writer, value.Pos, options);
        writer.WritePropertyName("Bd");
        JsonSerializer.Serialize<Body>(writer, value.Bd, options);
        writer.WritePropertyName("Attr");
        JsonSerializer.Serialize<Dictionary<string, Attributes_Attribute>>(writer, value.Attr, options);
        writer.WritePropertyName("Eq");
        JsonSerializer.Serialize<Equipment>(writer, value.Eq, options);
        writer.WriteNumber("PickUpCursor", value.PickUpCursor);
        writer.WriteEndObject();
    }
}
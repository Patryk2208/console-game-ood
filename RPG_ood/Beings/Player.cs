using System.Text;
using Attribute = RPG_ood.Attributes.Attribute;
using RPG_ood.Map;
using RPG_ood.Attributes;
using RPG_ood.Game;
using RPG_ood.Items;
using Attributes_Attribute = RPG_ood.Attributes.Attribute;


namespace RPG_ood.Beings;


public class Player : IBeing
{
    public string Name { get; init; }
    public bool IsDead { get; set; }
    public bool WasAttacked { get; set; }
    public Position Pos { get; set; }
    public AnsiConsoleColor Color { get; init; }
    public MomentChangedEvent MomentChangedEvent { get; init; }
    public Body Bd { get; }
    public Dictionary<string, Attributes_Attribute> Attr { get; protected set; }
    public Equipment Eq { get; protected set; }

    public Player(string name)
    {
        Name = name;
        Color = AnsiConsoleColor.Magenta;
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
    void IObserver.Update(GameState? state)
    {
        if (state == null) return;
        if (IsDead)
        {
            state.MomentChangedEvent.RemoveObserver(Name, this);
            state.CurrentRoom.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = -1, Y = -1 };
            return;
        }
        WasAttacked = false;
        MomentChangedEvent.NotifyObservers(state);
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
        var item = b.usedItem!;
        foreach (var bp in Bd.BodyParts.Values)
        {
            if (bp.usedItem == item)
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
            .Select(i => i.usedItem!);
        foreach (var it in manip)
        {
            it.AssignAttributes(res);
        }
        return res;
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

    public override string ToString()
    {
        return "¶";
    }
}
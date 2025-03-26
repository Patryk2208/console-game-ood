using System.Text;
using Attribute = RPG_ood.Attributes.Attribute;
using RPG_ood.Map;
using RPG_ood.Attributes;
using RPG_ood.Items;
using Attributes_Attribute = RPG_ood.Attributes.Attribute;


namespace RPG_ood.Beings;


public class Player : IBeing
{
    public string Name { get; init; }
    public Position Pos { get; set; }
    public int Color { get; init; }
    public Body Bd { get; }
    public Dictionary<string, Attributes_Attribute> Attr { get; protected set; }
    public Equipment Eq { get; protected set; }
    
    public Player(string name)
    {
        Name = name;
        Color = 35;
        Pos = new Position(0, 0);
        
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
    public override string ToString()
    {
        return "¶";
    }
}
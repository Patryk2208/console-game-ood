using Project_oob.Attributes;
using Project_oob.Items;
using Attribute = Project_oob.Attributes.Attribute;
using Project_oob.Map;


namespace Project_oob.Beings;


public class Player : UserControlledBeing
{
    //being type
    public Body Bd { get; }
    public Dictionary<string, Attribute> Attr { get; protected set; }
    public Equipment Eq { get; protected set; }
    
    public Player(string name)
    {
        Name = name;

        Pos = new Position(0, 0);
        
        Attr = new Dictionary<string, Attribute>();
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

    protected void PickUpItem(Item item)
    {
        Eq.AddItem(item);
    }
    
    protected void DropItem(int index)
    {
        var item = Eq.RemoveItem(index);
        item.SetPosition(Pos);
    }

    protected void PutOnItem(IUsable item, BodyPart part)
    {
        part.PutOn(item);
    }
    
    public override void TakeInput()
    {
        
    }


    public override string ToString()
    {
        return "¶";
    }
}
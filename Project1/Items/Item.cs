using Project_oob.Beings;
using Project_oob.Effects;
using Project_oob.Map;

namespace Project_oob.Items;

public abstract class Item : IMappable, IDecorable<Item>
{
    public string Name { get; protected set; }
    public Position Pos { get; set; }
    public int Color { get; protected set; }
    public Item Decorated { get; set; }
    public abstract void AssignAttributes(Dictionary<string, int> attributes);
    public abstract override string ToString();
    public abstract void Interact(Player p, Item? item);
    public abstract bool Apply(Body b, string bpName, Item? item);
    public abstract string PrintName();
}


public interface IValuable
{
    public int Value { get; protected set; }
}

public interface IPickupable
{
    
}

public interface IUsable
{
    
}
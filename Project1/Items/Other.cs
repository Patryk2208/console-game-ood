using Project_oob.Beings;
using Project_oob.Effects;

namespace Project_oob.Items;

public abstract class Mineral : Item, IDecorable<Mineral>
{
    public int Quantity { get; protected set; }
    public new Mineral Decorated { get; set; }
    public override bool Apply(Body b, string bpName) { return false; }
    public override void Interact(Player p)
    {
        p.Eq.AddItemToEq(this);
    }
    public override void AssignAttributes(Dictionary<string, int> attributes) { }
    public abstract override string ToString();
    public override string PrintName() => $"{Name} [{Quantity}]";
}

public class Sand : Mineral
{
    public Sand()
    {
        Name = "Sand";
        Quantity = 1;
    }
    public override string ToString()
    {
        return "*";
    }
}

public class Wood : Mineral
{
    public Wood()
    {
        Name = "Wood";
        Quantity = 1;
    }

    public override string ToString()
    {
        return "@";
    }
}

public class Water : Mineral
{
    public Water()
    {
        Name = "Water";
        Quantity = 1;
    }

    public override string ToString()
    {
        return "?";
    }
}
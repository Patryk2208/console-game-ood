using Project_oob.Beings;

namespace Project_oob.Items;

public abstract class Mineral : Item
{
    public int Quantity { get; protected set; }
    public override bool Apply(Body b, string bpName, Item? item) { return false; }
    public override void Interact(Player p, Item? item)
    {
        p.Eq.AddItemToEq(item ?? this);
    }
    public override void AssignAttributes(Dictionary<string, int> attributes)
    {
        throw new NotImplementedException();
    }
    public abstract override string ToString();
    public override string PrintName() => Name;
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
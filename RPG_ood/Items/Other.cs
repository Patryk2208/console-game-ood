using RPG_ood.Effects;
using RPG_ood.Beings;

namespace RPG_ood.Items;

public interface IMineral : IItem, IPickupable
{
    public int Quantity { get; protected set; }
    bool IPickupable.Apply(Body b, string bpName) { return false; }
    void IItem.Interact(Player p)
    {
        p.Eq.AddItemToEq(this);
    }
    string IItem.PrintName() => $"{Name} [{Quantity}]";
}

public abstract class Mineral : IMineral
{
    public Position Pos { get; set; }
    public string Name { get; set; }
    public int Color { get; set; }
    public int Quantity { get; set; }
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
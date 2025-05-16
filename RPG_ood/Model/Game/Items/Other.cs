using System.Text.Json.Serialization;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game.Beings;
using RPG_ood.Model.Game.Items;
using RPG_ood.View;

namespace RPG_ood.Model.Items;

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

[Serializable]
public abstract class Mineral : IMineral
{
    public Position Pos { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    
    public Mineral() {}

    [JsonConstructor]
    public Mineral(string name, int quantity, Position pos)
    {
        Name = name;
        Quantity = quantity;
        Pos = pos;
    }

    public abstract void AcceptView(IViewGenerator generator);
}

public class Sand : Mineral
{
    public Sand()
    {
        Name = "Sand";
        Quantity = 1;
    }
    
    [JsonConstructor]
    public Sand(string name, int quantity, Position pos) : base(name, quantity, pos) {}

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitSand(this);
    }
}

public class Wood : Mineral
{
    public Wood()
    {
        Name = "Wood";
        Quantity = 1;
    }
    
    [JsonConstructor]
    public Wood(string name, int quantity, Position pos) : base(name, quantity, pos) {}

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitWood(this);
    }
}

public class Water : Mineral
{
    public Water()
    {
        Name = "Water";
        Quantity = 1;
    }
    
    [JsonConstructor]
    public Water(string name, int quantity, Position pos) : base(name, quantity, pos) {}

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitWater(this);
    }
}
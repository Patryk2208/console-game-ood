using System.Text.Json.Serialization;
using Model.Game.Beings;
using Model.GenerateView;

namespace Model.Game.Items;

public interface IMoney : IItem, IValuable
{
    string IItem.PrintName() => $"{Name} [{Value}]";
}

[Serializable]
public abstract class Money : IMoney
{
    public string Name { get; set; }
    public Position Pos { get; set; }
    public int Value { get; set; }
    
    public Money() {}

    [JsonConstructor]
    public Money(string name, int value, Position pos)
    {
        Name = name;
        Value = value;
        Pos = pos;
    }
    public abstract void Interact(Player p);

    public abstract void AcceptView(IViewGenerator generator);
}

public class Coin : Money
{
    public Coin()
    {
        Name = "Coin";
        Value = 2;
    }
    
    [JsonConstructor]
    public Coin(string name, int value, Position pos) : base(name, value, pos) {}

    public override void Interact(Player p)
    {
        p.Eq.CoinCount++;
        p.Eq.AddItemToSack(this);
    }

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitCoin(this);
    }
}

public class Gold : Money
{
    public Gold()
    {
        Name = "Gold";
        Value = 5;
    }

    [JsonConstructor]
    public Gold(string name, int value, Position pos) : base(name, value, pos) {}
    
    public override void Interact(Player p)
    {
        p.Eq.GoldCount++;
        p.Eq.AddItemToSack(this);
    }

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitGold(this);
    }
}
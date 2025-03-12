using Project_oob.Beings;
using Project_oob.Effects;

namespace Project_oob.Items;

public abstract class Money : Item, IValuable, IDecorable<Money>
{
    public int  Value { get; set; }

    public abstract override void Interact(Player p);
    public new Money Decorated { get; set; } = null!;
    public override void AssignAttributes(Dictionary<string, int> attributes) {}
    public override bool Apply(Body b, string bpName) { return false; }
    public override string PrintName() => $"{Name} [{Value}]";
}

public class Coin : Money
{
    public Coin()
    {
        Name = "Coin";
        Color = 93;
        Value = 2;
    }

    public override void Interact(Player p)
    {
        p.Eq.CoinCount++;
        p.Eq.AddItemToSack(this);
    }

    public override string ToString()
    {
        return "\u00a9";
    }
}

public class Gold : Money
{
    public Gold()
    {
        Name = "Gold";
        Color = 33;
        Value = 5;
    }

    public override void Interact(Player p)
    {
        p.Eq.GoldCount++;
        p.Eq.AddItemToSack(this);
    }
    public override string ToString()
    {
        return "G";
    }
}
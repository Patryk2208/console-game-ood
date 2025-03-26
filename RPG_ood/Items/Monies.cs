using RPG_ood.Effects;
using RPG_ood.Beings;

namespace RPG_ood.Items;

public interface IMoney : IItem, IValuable
{
    public int Value { get; set; }
    string IItem.PrintName() => $"{Name} [{Value}]";
}

public class Coin : IMoney
{
    public string Name { get; set; }
    public int Color { get; set; }
    public Position Pos { get; set; }
    public int Value { get; set; }
    public Coin()
    {
        Name = "Coin";
        Color = 93;
        Value = 2;
    }

    public void Interact(Player p)
    {
        p.Eq.CoinCount++;
        p.Eq.AddItemToSack(this);
    }

    public override string ToString()
    {
        return "\u00a9"; 
    }
}

public class Gold : IMoney
{
    public string Name { get; set; }
    public int Color { get; set; }
    public Position Pos { get; set; }
    public int Value { get; set; }
    public Gold()
    {
        Name = "Gold";
        Color = 33;
        Value = 5;
    }

    public void Interact(Player p)
    {
        p.Eq.GoldCount++;
        p.Eq.AddItemToSack(this);
    }
    public override string ToString()
    {
        return "$";
    }
}
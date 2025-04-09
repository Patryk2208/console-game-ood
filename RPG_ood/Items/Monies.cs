using RPG_ood.Effects;
using RPG_ood.Beings;

namespace RPG_ood.Items;

public interface IMoney : IItem, IValuable
{
    string IItem.PrintName() => $"{Name} [{Value}]";
}

public abstract class Money : IMoney
{
    public string Name { get; set; }
    public AnsiConsoleColor Color { get; set; } = AnsiConsoleColor.BrightYellow;
    public abstract void Interact(Player p);
    public Position Pos { get; set; }
    public int Value { get; set; }
}

public class Coin : Money
{
    public Coin()
    {
        Name = "Coin";
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
        Value = 5;
    }

    public override void Interact(Player p)
    {
        p.Eq.GoldCount++;
        p.Eq.AddItemToSack(this);
    }
    public override string ToString()
    {
        return "$";
    }
}
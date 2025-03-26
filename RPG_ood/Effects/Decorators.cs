using RPG_ood.Beings;
using RPG_ood.Items;

namespace RPG_ood.Effects;

public abstract class MineralDecorator : Mineral
{
    public Mineral Decorated {get; protected set;}
    protected MineralDecorator(Mineral item)
    {
        Decorated = item;
        Pos = Decorated.Pos;
        Color = Decorated.Color;
    }
    public override string ToString()
    {
        return Decorated.ToString();
    }
}

public class MagicMineral : MineralDecorator
{
    public MagicMineral(Mineral item) : base(item)
    {
        Quantity = Decorated.Quantity * 3;
        Name = "(Magic) " + Decorated.Name;
    }
}
using Project_oob.Beings;
using Project_oob.Items;

namespace Project_oob.Effects;

public interface IDecorable<T> where T : class
{
    public T Decorated { get; set; }
}

public abstract class MineralDecorator : Mineral
{
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
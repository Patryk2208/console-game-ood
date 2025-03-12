using Project_oob.Beings;
using Project_oob.Items;

namespace Project_oob.Effects;

public interface IDecorable<T> where T : class
{
    public T Decorated { get; set; }
}

public abstract class MoneyDecorator : Money
{
    protected MoneyDecorator(Money item)
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

public class PreciousMoney : MoneyDecorator
{
    public PreciousMoney(Money item) : base(item)
    {
        Value = Decorated.Value * 2;
        Name = "(Precious) " + Decorated.Name;
    }
}
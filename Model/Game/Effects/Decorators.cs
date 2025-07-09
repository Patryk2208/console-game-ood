using Model.Game.Items;
using Model.GenerateView;

namespace Model.Game.Effects;

[Serializable]
public abstract class MineralDecorator : Mineral
{
    public Mineral Decorated {get; protected set;}
    protected MineralDecorator(Mineral item)
    {
        Decorated = item;
        Pos = Decorated.Pos;
    }

    public override void AcceptView(IViewGenerator generator)
    {
        Decorated.AcceptView(generator);
    }
}

[Serializable]
public class MagicMineral : MineralDecorator
{
    public MagicMineral(Mineral item) : base(item)
    {
        Quantity = Decorated.Quantity * 3;
        Name = "(Magic) " + Decorated.Name;
    }
}
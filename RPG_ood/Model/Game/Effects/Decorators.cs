using RPG_ood.Model.Items;
using RPG_ood.View;

namespace RPG_ood.Model.Effects;

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
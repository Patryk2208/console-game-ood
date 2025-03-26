using RPG_ood.Effects;
using RPG_ood.Beings;
using RPG_ood.Map;

namespace RPG_ood.Items;

public interface IItem : IMappable
{
    public string Name { get; set; }
    public int Color { get; set; }
    public void Interact(Player p);
    public string PrintName();
}


public interface IValuable : IItem
{
    public int Value { get; protected set; }
}

public interface IPickupable : IItem
{
    public bool Apply(Body b, string bpName);
}

public interface IUsable : IPickupable
{
    public void AssignAttributes(Dictionary<string, int> attributes);
}
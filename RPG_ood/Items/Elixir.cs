using RPG_ood.Beings;

namespace RPG_ood.Items;

public interface IElixir : IUsable, IPickupable, IItem
{
    
}

public abstract class Elixir : IElixir
{
    public Position Pos { get; set; }
    public string Name { get; set; }
    public int Color { get; set; }
    public void Interact(Player p)
    {
        p.Eq.AddItemToEq(this);
    }

    public string PrintName()
    {
        return $"{Name}";
    }
    public bool Apply(Body b, string bpName)
    {
        throw new NotImplementedException();
    }
    public abstract void AssignAttributes(Dictionary<string, int> attributes);
}

public class HealthElixir : Elixir
{
    public HealthElixir()
    {
        Name = "Health Elixir";
        Color = 33;
    }
    public override void AssignAttributes(Dictionary<string, int> attributes)
    {
        if (attributes.TryGetValue("Health", out int health))
        {
            health += 10;
            attributes["Health"] = health;
        }
    }

    public override string ToString()
    {
        return "e";
    }
}
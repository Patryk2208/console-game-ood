using RPG_ood.Map;

namespace RPG_ood.Beings;

public interface IBeing : IMappable
{
    public string Name { get; protected init; }
    public int Color { get; protected init; }
}

public interface INpc : IBeing
{
    public int Hp { get; set; }
    public void TakeDamage(int damage);
}
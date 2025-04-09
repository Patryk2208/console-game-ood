using RPG_ood.Game;
using RPG_ood.Map;

namespace RPG_ood.Beings;

public interface IBeing : IMappable, IObserver
{
    public string Name { get; protected init; }
    public AnsiConsoleColor Color { get; protected init; }
    protected MomentChangedEvent MomentChangedEvent { get; init; }
}

public interface INpc : IBeing
{
    public void TakeDamage(int damage);
    public void Wander(Room room);
}
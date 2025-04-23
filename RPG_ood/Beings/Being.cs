using RPG_ood.Game;
using RPG_ood.Map;

namespace RPG_ood.Beings;

public interface IBeing : IMappable, IObserver
{
    public string Name { get; protected init; }
    public bool IsDead { get; protected set; }
    public AnsiConsoleColor Color { get; protected init; }
    protected MomentChangedEvent MomentChangedEvent { get; init; } 
    public void ReceiveDamage(int damage);
    public void Die();
    public IEnemy? CanFight();
}

public interface INpc : IBeing
{
    public void Wander(Room room);
}
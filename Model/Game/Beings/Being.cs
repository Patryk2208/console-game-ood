using System.Text.Json.Serialization;
using Model.Game.GameState;
using Model.Game.Map;
using Model.GenerateView;

namespace Model.Game.Beings;

[JsonDerivedType(typeof(Orc), "Orc")]
[JsonDerivedType(typeof(Giant), "Giant")]
public interface IBeing : IMappable, IObserver
{
    public string Name { get; protected init; }
    public bool IsDead { get; protected set; }
    public bool WasAttacked { get; set; }
    [JsonIgnore]
    protected MomentChangedEvent MomentChangedEvent { get; init; } 
    public void ReceiveDamage(int damage);
    public void Die();
    public IEnemy? CanFight();
    public void AcceptView(IViewGenerator generator);
}

public interface INpc : IBeing
{
    public MovementStrategy Strategy { get; set; }
}
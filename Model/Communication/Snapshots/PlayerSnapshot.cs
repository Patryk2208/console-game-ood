using System.Text.Json.Serialization;
using Model.Game;
using Model.Game.Beings;
using Model.GenerateView;

namespace Model.Communication.Snapshots;

[Serializable]
public class PlayerSnapshot
{
    public long ID { get; set; }
    public string Name { get; init; }
    public bool IsDead { get; set; }
    public bool WasAttacked { get; set; }
    public Position Pos { get; set; }

    public PlayerSnapshot(Player player)
    {
        ID = 0;
        Name = player.Name;
        IsDead = player.IsDead;
        WasAttacked = player.WasAttacked;
        Pos = player.Pos;
    }

    [JsonConstructor]
    public PlayerSnapshot(long id, string name, bool isDead, bool wasAttacked, Position pos)
    {
        ID = id;
        Name = name;
        IsDead = isDead;
        WasAttacked = wasAttacked;
        Pos = pos;
    }

    public void AcceptView(IViewGenerator viewGenerator)
    {
        viewGenerator.VisitPlayerSnapshot(this);
    }
}
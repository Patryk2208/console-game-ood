using Model.Game;

namespace Model.RelativeGameState;

public class RelativePlayerState
{
    public long ID { get; set; }
    public string Name { get; init; }
    public bool IsDead { get; set; }
    public bool WasAttacked { get; set; }
    public Position Pos { get; set; }
}
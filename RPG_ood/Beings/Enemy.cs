using System.Runtime.CompilerServices;
using RPG_ood.Game;
using RPG_ood.Map;

namespace RPG_ood.Beings;

public interface IEnemy : INpc
{
    public void Attack();
}

public abstract class Enemy : IEnemy
{
    public Position Pos { get; set; }
    public string Name { get; init; }
    public AnsiConsoleColor Color { get; init; }
    public MomentChangedEvent MomentChangedEvent { get; init; }
    private int MomentPassed { get; set; } = 0;
    public int MomentInterval {get; set;}
    private Random _random { get; set; } = new();
    private List<Action<Room>> PossibleMovements { get; set; }
    protected Enemy()
    {
        PossibleMovements =
        [
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight
        ];
        MomentChangedEvent = null!; //Todo
    }
    void IObserver.Update(GameState? state)
    {
        if (state == null)
        {
            return;
        }
        ++MomentPassed;
        if (MomentPassed % MomentInterval == 0)
        {
            Wander(state!.CurrentRoom);
        }
    }
    public void Wander(Room room)
    {
        PossibleMovements[_random.Next(PossibleMovements.Count)].Invoke(room);
    }
    protected void MoveUp(Room room)
    {
        if (Pos.X - 1 >= 0 && room.Elements[Pos.X - 1, Pos.Y].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X - 1, Y = Pos.Y };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Up";
        }
    }
    protected void MoveDown(Room room)
    {
        if (Pos.X + 1 < room.Height && room.Elements[Pos.X + 1, Pos.Y].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X + 1, Y = Pos.Y };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Up";
        }
    }
    
    protected void MoveLeft(Room room)
    {
        if (Pos.Y - 1 >= 0 && room.Elements[Pos.X, Pos.Y - 1].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X, Y = Pos.Y - 1 };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Up";
        }
    }
    
    protected void MoveRight(Room room)
    {
        if (Pos.Y + 1 < room.Width && room.Elements[Pos.X, Pos.Y + 1].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X, Y = Pos.Y + 1 };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Up";
        }
    }
    public void TakeDamage(int damage)
    {
        throw new NotImplementedException();
    }
    public void Attack()
    {
        throw new NotImplementedException();
    }
}

public class Orc : Enemy
{

    public Orc()
    {
        Name = "Orc";
        Color = AnsiConsoleColor.Green;
        MomentInterval = 8;
    }
    public override string ToString()
    {
        return "O";
    }
}
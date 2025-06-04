using System.Text.Json.Serialization;
using RPG_ood.Map;
using RPG_ood.Model.Game.GameState;
using RPG_ood.View;

namespace RPG_ood.Model.Game.Beings;

public interface IEnemy : INpc
{
    public int Health { get; set; }
    public int InitialHealth { get; set; }
    public int Armor { get; set; }
    public int Damage { get; set; }
    [JsonIgnore]
    public List<Func<Room, bool>> PossibleMovements { get; }
    IEnemy? IBeing.CanFight()
    {
        return this;
    }
}

[Serializable]
public abstract class Enemy : IEnemy
{
    public Position Pos { get; set; }
    public string Name { get; init; }
    public bool IsDead { get; set; }
    public bool WasAttacked { get; set; }
    public MovementStrategy Strategy { get; set; }
    [JsonIgnore]
    public MomentChangedEvent MomentChangedEvent { get; init; }
    [JsonIgnore]
    private int MomentPassed { get; set; } = 0;
    [JsonIgnore]
    public int MomentInterval {get; set;}
    [JsonIgnore]
    public List<Func<Room, bool>> PossibleMovements { get; set; }
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
    void IObserver.Update(GameState.GameState? state, long id)
    {
        if (state == null)
        {
            return;
        }
        ++MomentPassed;
        if (IsDead)
        {
            state.MomentChangedEvent.RemoveObserver(Name, this);
            state.CurrentRoom.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = -1, Y = -1 };
            return;
        }
        if (MomentPassed % MomentInterval == 0)
        {
            AlterStrategy();
            Strategy.Wander(state!.CurrentRoom, this);
        }
        WasAttacked = false;
    }

    private void AlterStrategy()
    {
        if (Health < InitialHealth && Health >= InitialHealth * 0.5)
        {
            Strategy = new AggressiveMovementStrategy();
        }
        else if (Health < InitialHealth * 0.5)
        {
            Strategy = new ShyMovementStrategy();
        }
    }
    
    protected bool MoveUp(Room room)
    {
        if (Pos.X - 1 >= 0 && room.Elements[Pos.X - 1, Pos.Y].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X - 1, Y = Pos.Y };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Up";
            return true;
        }
        return false;
    }

    protected bool MoveDown(Room room)
    {
        if (Pos.X + 1 < room.Height && room.Elements[Pos.X + 1, Pos.Y].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X + 1, Y = Pos.Y };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Up";
            return true;
        }
        return false;
    }
    
    protected bool MoveLeft(Room room)
    {
        if (Pos.Y - 1 >= 0 && room.Elements[Pos.X, Pos.Y - 1].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X, Y = Pos.Y - 1 };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Up";
            return true;
        }
        return false;
    }

    protected bool MoveRight(Room room)
    {
        if (Pos.Y + 1 < room.Width && room.Elements[Pos.X, Pos.Y + 1].OnStandable)
        {
            room.Elements[Pos.X, Pos.Y].OnStandable = true;
            Pos = Pos with { X = Pos.X, Y = Pos.Y + 1 };
            room.Elements[Pos.X, Pos.Y].OnStandable = false;
            //newMessage = $"{P.Name} Moved Up";
            return true;
        }
        return false;
    }
    public int Health { get; set; }
    public int InitialHealth { get; set; }
    public int Armor { get; set; }
    public int Damage { get; set; }

    public void ReceiveDamage(int damage)
    {
        Health -= damage;
        if (damage > 0)
        {
            WasAttacked = true;
        }
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        IsDead = true;
    }
    
    public abstract void AcceptView(IViewGenerator generator);
}

public class Orc : Enemy
{

    public Orc(MovementStrategy strategy)
    {
        Name = "Orc";
        MomentInterval = 5;
        InitialHealth = Health = 100;
        Armor = 0;
        Damage = 30;
        Strategy = strategy;
    }

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitOrc(this);
    }
}

public class Giant : Enemy
{

    public Giant(MovementStrategy strategy)
    {
        Name = "Giant";
        MomentInterval = 10;
        InitialHealth = Health = 200;
        Armor = 50;
        Damage = 60;
        Strategy = strategy;
    }

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitGiant(this);
    }
}
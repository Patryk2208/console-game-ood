using System.Text.Json.Serialization;
using Model.Game.Attack;
using Model.Game.Map;

namespace Model.Game.Beings;

[Serializable]
[JsonPolymorphic]
[JsonDerivedType(typeof(CalmMovementStrategy), nameof(CalmMovementStrategy))]
[JsonDerivedType(typeof(RoamingMovementStrategy), nameof(RoamingMovementStrategy))]
[JsonDerivedType(typeof(AggressiveMovementStrategy), nameof(AggressiveMovementStrategy))]
[JsonDerivedType(typeof(ShyMovementStrategy), nameof(ShyMovementStrategy))]
public abstract class MovementStrategy(float surroundingRadius)
{
    public float SurroundingRadius { get; set; } = surroundingRadius;
    public abstract void Wander(Room room, IEnemy enemy);
}

public class CalmMovementStrategy(float surroundingRadius = 2.5f) : MovementStrategy(surroundingRadius)
{
    public override void Wander(Room room, IEnemy enemy)
    {
        if (enemy.WasAttacked)
        {
            var attackable = room.GetPlayersNearby(enemy.Pos, 2.5f); //2.5 - one each way
            var p = attackable.First();
            Fight f = new EnemyPlayerFight(enemy, p);
            f.Attack();
        }
    }
}

public class RoamingMovementStrategy(float surroundingRadius = 2.5f) : MovementStrategy(surroundingRadius)
{
    public override void Wander(Room room, IEnemy enemy)
    {
        if (enemy.WasAttacked)
        {
            var attackable = room.GetPlayersNearby(enemy.Pos, 2.5f); //2.5 - one each way
            var p = attackable.First();
            Fight f = new EnemyPlayerFight(enemy, p);
            f.Attack();
        }
        var random = new Random();
        int safety = 0;
        while (!enemy.PossibleMovements[random.Next(4)].Invoke(room) && safety++ < 10) ;
    }
}

public class AggressiveMovementStrategy(float surroundingRadius = 100) : MovementStrategy(surroundingRadius)
{
    public override void Wander(Room room, IEnemy enemy)
    {
        var surroundings = room.GetPlayersNearby(enemy.Pos, SurroundingRadius);
        Player? closestPlayer = null;
        float closestDistance = float.MaxValue;
        foreach (var p in surroundings)
        {
            if (closestPlayer == null || enemy.Pos.Distance(p.Pos) < closestDistance)
            {
                closestDistance = enemy.Pos.Distance(p.Pos);
                closestPlayer = p;
            }
        }
        if(closestPlayer == null) return;

        var directions = ((int[])
        [
            enemy.Pos.X - closestPlayer.Pos.X, closestPlayer.Pos.X - enemy.Pos.X,
            enemy.Pos.Y - closestPlayer.Pos.Y, closestPlayer.Pos.Y - enemy.Pos.Y
        ]);
        int maxv = directions[0], max = 0;
        for (int i = 1; i < directions.Length; i++)
        {
            if (directions[i] > maxv)
            {
                maxv = directions[i];
                max = i;
            }
        }
        int max2 = max == 0 ? 1 : 0;
        int max2v = directions[max2];
        for (int i = 0; i < directions.Length; i++)
        {
            if (max != i && directions[i] > max2)
            {
                max2v = directions[i];
                max2 = i;
            }
        }

        if (!enemy.PossibleMovements[max].Invoke(room))
        {
            if (max2v > 0)
            {
                enemy.PossibleMovements[max2].Invoke(room);
            }
        }
        
        var attackable = room.GetPlayersNearby(enemy.Pos, 2.5f); //2.5 - one each way
        if (attackable.Contains(closestPlayer))
        {
            Fight f = new EnemyPlayerFight(enemy, closestPlayer);
            f.Attack();
        }
    }
}

public class ShyMovementStrategy(float surroundingRadius = 100) : MovementStrategy(surroundingRadius)
{
    public override void Wander(Room room, IEnemy enemy)
    {
        var surroundings = room.GetPlayersNearby(enemy.Pos, SurroundingRadius);
        Player? closestPlayer = null;
        float closestDistance = float.MaxValue;
        foreach (var p in surroundings)
        {
            if (closestPlayer == null || enemy.Pos.Distance(p.Pos) < closestDistance)
            {
                closestDistance = enemy.Pos.Distance(p.Pos);
                closestPlayer = p;
            }
        }
        if(closestPlayer == null) return;

        var directions = ((int[])
        [
            enemy.Pos.X - closestPlayer.Pos.X, closestPlayer.Pos.X - enemy.Pos.X,
            enemy.Pos.Y - closestPlayer.Pos.Y, closestPlayer.Pos.Y - enemy.Pos.Y
        ]);


        int minv = directions[0], min = 0;
        for (int i = 1; i < directions.Length; i++)
        {
            if (directions[i] < minv)
            {
                minv = directions[i];
                min = i;
            }
        }
        int min2 = min == 0 ? 1 : 0;
        int min2v = directions[min2];
        for (int i = 0; i < directions.Length; i++)
        {
            if (min != i && directions[i] < min2)
            {
                min2v = directions[i];
                min2 = i;
            }
        }

        if (!enemy.PossibleMovements[min].Invoke(room))
        {
            if (min2v < 0)
            {
                enemy.PossibleMovements[min2].Invoke(room);
            }
        }
    }
}
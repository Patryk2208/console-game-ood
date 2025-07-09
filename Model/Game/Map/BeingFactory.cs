using Model.Game.Beings;

namespace Model.Game.Map;

public interface IBeingFactory
{
    IBeing CreateBeing();
}

public class EnemyFactory (Random seed) : IBeingFactory
{
    private Random _seed { get; } = seed;
    private List<Func<MovementStrategy, IEnemy>> CreateFunctions { get; set; } = 
    [
        (MovementStrategy strategy) => new Orc(strategy),
        (MovementStrategy strategy) => new Giant(strategy)
    ];

    private List<MovementStrategy> PossibleStrategies { get; set; } =
    [
        new CalmMovementStrategy(),
        new RoamingMovementStrategy(),
        new AggressiveMovementStrategy(),
        new ShyMovementStrategy()
    ];
    public IBeing CreateBeing()
    {
        return CreateFunctions[_seed.Next(CreateFunctions.Count)]
            .Invoke(PossibleStrategies[_seed.Next(PossibleStrategies.Count)]);
    }
}
using RPG_ood.Beings;

namespace RPG_ood.Map;

public interface IBeingFactory
{
    IBeing CreateBeing();
}

public class EnemyFactory (Random seed) : IBeingFactory
{
    private Random _seed { get; } = seed;
    private List<Func<IEnemy>> CreateFunctions { get; set; } = 
    [
        () => new Orc(),
        () => new Giant()
    ];
    public IBeing CreateBeing()
    {
        return CreateFunctions[_seed.Next(CreateFunctions.Count)].Invoke();
    }
}
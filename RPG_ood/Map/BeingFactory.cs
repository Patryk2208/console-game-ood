using RPG_ood.Beings;

namespace RPG_ood.Map;

public interface IBeingFactory
{
    IBeing CreateBeing();
}

public class EnemyFactory (Random seed) : IBeingFactory
{
    private Random _seed { get; } = seed;
    public IBeing CreateBeing()
    {
        IEnemy enemy;
        switch (_seed.Next(1))
        {
            case 0:
            {
                enemy = CreateOrc();
                break;
            }
            default:
            {
                throw new Exception("Random.Next() error.");
            }
        }
        return enemy;
    }
    private Orc CreateOrc() => new Orc();
}
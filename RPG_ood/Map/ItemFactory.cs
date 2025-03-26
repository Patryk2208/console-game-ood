using RPG_ood.Effects;
using RPG_ood.Items;

namespace RPG_ood.Map;

public interface ItemFactory
{
    public IItem CreateItem();
}

public class MoneyFactory (Random seed): ItemFactory
{
    private Random _seed { get; } = seed;
    public IItem CreateItem()
    {
        IMoney item;
        switch (_seed.Next(2))
        {
            case 0:
            {
                item = CreateCoin();
                break;
            }
            case 1:
            {
                item = CreateGold();
                break;
            }
            default:
            {
                throw new Exception("Random.Next() error.");
            }
        }
        return item;
    }
    private Gold CreateGold() => new Gold();
    private Coin CreateCoin() => new Coin();
}

public class ElixirFactory (Random seed) : ItemFactory
{
    private Random _seed { get; } = seed;
    public IItem CreateItem()
    {
        IElixir item;
        switch (_seed.Next(1))
        {
            case 0:
            {
                item = CreateHealthElixir();
                break;
            }
            default:
            {
                throw new Exception("Random.Next() error.");
            }
        }
        return item;
    }
    private HealthElixir CreateHealthElixir() => new HealthElixir();
}

public class MineralFactory (Random seed): ItemFactory
{
    private Random _seed { get; } = seed;
    public IItem CreateItem()
    {
        IMineral item;
        switch (_seed.Next(3))
        {
            case 0:
            {
                item = CreateSand();
                break;
            }
            case 1:
            {
                item = CreateWood();
                break;
            }
            case 2:
            {
                item = CreateWater();
                break;
            }
            default:
            {
                throw new Exception("Random.Next() error.");
            }
        }
        return item;
    }
    private Sand CreateSand() => new Sand();
    private Wood CreateWood() => new Wood();
    private Water CreateWater() => new Water();
}

public class WeaponFactory (Random seed, bool modified): ItemFactory
{
    private Random _seed { get; } = seed;
    private bool _modified { get; } = modified;
    public IItem CreateItem()
    {
        IWeapon item;
        switch (_seed.Next(4))
        {
            case 0:
            {
                item = CreateSword();
                break;
            }
            case 1:
            {
                item = CreateKnife();
                break;
            }
            case 2:
            {
                item = CreateShield();
                break;
            }
            case 3:
            {
                item = CreateBigSword();
                break;
            }
            default:
            {
                throw new Exception("Random.Next() error.");
            }
        }

        if (_modified)
        {
            int noMods = _seed.Next(1, 3);
            for (int i = 0; i < noMods; i++)
            {
                switch (_seed.Next(6))
                {
                    case 0:
                    {
                        item = AddStrongEffect(item);
                        break;
                    }
                    case 1:
                    {
                        item = AddLuckyEffect(item);
                        break;
                    }
                    case 2:
                    {
                        item = AddDefensiveEffect(item);
                        break;
                    }
                    case 3:
                    {
                        item = AddOffensiveEffect(item);
                        break;
                    }
                    case 4:
                    {
                        item = AddHeavyEffect(item);
                        break;
                    }
                    case 5:
                    {
                        item = AddLightEffect(item);
                        break;
                    }
                    default:
                    {
                        throw new Exception("Random.Next() error.");
                    }
                }
            }
        }
        return item;
    }
    private Sword CreateSword() => new Sword();
    private Knife CreateKnife() => new Knife();
    private Shield CreateShield() => new Shield();
    private BigSword CreateBigSword() => new BigSword();
    
    //effects
    private IWeapon AddStrongEffect(IWeapon weapon) => new StrongWeapon(weapon);
    private IWeapon AddLuckyEffect(IWeapon weapon) => new LuckyWeapon(weapon);
    private IWeapon AddDefensiveEffect(IWeapon weapon) => new DefensiveWeapon(weapon);
    private IWeapon AddOffensiveEffect(IWeapon weapon) => new OffensiveWeapon(weapon);
    private IWeapon AddHeavyEffect(IWeapon weapon) => new HeavyWeapon(weapon);
    private IWeapon AddLightEffect(IWeapon weapon) => new LightWeapon(weapon);

}
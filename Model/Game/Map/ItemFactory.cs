using Model.Game.Effects;
using Model.Game.Items;

namespace Model.Game.Map;

public interface ItemFactory
{
    public IItem CreateItem();
    
}
public class MoneyFactory (Random seed): ItemFactory
{
    private Random _seed { get; } = seed;
    private List<Func<IMoney>> CreateFunctions { get; set; } = 
    [
        () => new Coin(),
        () => new Gold()
    ];
    
    public IItem CreateItem()
    {
        return CreateFunctions[_seed.Next(CreateFunctions.Count)].Invoke();
    }
}

public class ElixirFactory (Random seed) : ItemFactory
{
    private Random _seed { get; } = seed;
    private List<Func<IElixir>> CreateFunctions { get; set; } = 
    [
        () => new PowerElixir(),
        () => new HealingElixir(),
        () => new Poison(),
        () => new Antidote(),
        () => new HealthElixir()
    ];
    public IItem CreateItem()
    {
        return CreateFunctions[_seed.Next(CreateFunctions.Count)].Invoke();
    }
}

public class MineralFactory (Random seed): ItemFactory
{
    private Random _seed { get; } = seed;
    private List<Func<IMineral>> CreateFunctions { get; set; } = 
    [
        () => new Sand(),
        () => new Wood(),
        () => new Water()
    ];
    public IItem CreateItem()
    {
        return CreateFunctions[_seed.Next(CreateFunctions.Count)].Invoke();
    }
}

public class WeaponFactory (Random seed, int modified): ItemFactory
{
    private Random _seed { get; } = seed;
    private int _modified { get; } = modified;
    private List<Func<IWeapon>> CreateFunctions { get; set; } = 
    [
        () => new Sword(),
        () => new Knife(),
        () => new Shield(),
        () => new BigSword()
    ];

    private List<Func<IWeapon, IWeapon>> ModifyWeapons { get; set; } =
    [
        weapon => new StrongWeapon(weapon),
        weapon => new LuckyWeapon(weapon),
        weapon => new DefensiveWeapon(weapon),
        weapon => new OffensiveWeapon(weapon),
        weapon => new SlowWeapon(weapon),
        weapon => new FastWeapon(weapon)
    ];
    public IItem CreateItem()
    {
        var item = CreateFunctions[_seed.Next(CreateFunctions.Count)].Invoke();
        for (int i = 0; i < _modified; i++)
        {
            item = ModifyWeapons[_seed.Next(ModifyWeapons.Count)].Invoke(item);
        }
        return item;
    }

}
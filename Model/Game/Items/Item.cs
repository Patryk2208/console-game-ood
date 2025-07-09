using System.Text.Json.Serialization;
using Model.Game.Attack;
using Model.Game.Beings;
using Model.Game.Effects;
using Model.Game.Map;
using Model.GenerateView;

namespace Model.Game.Items;

[JsonPolymorphic]
//
[JsonDerivedType(typeof(Water), "Water")]
[JsonDerivedType(typeof(Wood), "Wood")]
[JsonDerivedType(typeof(Sand), "Sand")]
[JsonDerivedType(typeof(MagicMineral), "MagicMineral")]
//
[JsonDerivedType(typeof(Gold), "Gold")]
[JsonDerivedType(typeof(Coin), "Coin")]
//
[JsonDerivedType(typeof(HealthElixir), "HealthElixir")]
[JsonDerivedType(typeof(HealingElixir), "HealingElixir")]
[JsonDerivedType(typeof(PowerElixir), "PowerElixir")]
[JsonDerivedType(typeof(Poison), "Poison")]
[JsonDerivedType(typeof(Antidote), "Antidote")]
//
[JsonDerivedType(typeof(Sword), "Sword")]
[JsonDerivedType(typeof(Knife), "Knife")]
[JsonDerivedType(typeof(BigSword), "BigSword")]
[JsonDerivedType(typeof(Shield), "Shield")]
//
[JsonDerivedType(typeof(StrongWeapon), "StrongWeapon")]
[JsonDerivedType(typeof(LuckyWeapon), "LuckyWeapon")]
[JsonDerivedType(typeof(DefensiveWeapon), "DefensiveWeapon")]
[JsonDerivedType(typeof(OffensiveWeapon), "OffensiveWeapon")]
[JsonDerivedType(typeof(SlowWeapon), "SlowWeapon")]
[JsonDerivedType(typeof(FastWeapon), "FastWeapon")]
public interface IItem : IMappable
{
    public string Name { get; set; }
    public void Interact(Player p);
    public string PrintName();
    public void AcceptView(IViewGenerator generator);
}


[JsonPolymorphic]
[JsonDerivedType(typeof(Gold), "Gold")]
[JsonDerivedType(typeof(Coin), "Coin")]
public interface IValuable : IItem
{
    public int Value { get; protected set; }
}


[JsonPolymorphic]
[JsonDerivedType(typeof(Water), "Water")]
[JsonDerivedType(typeof(Wood), "Wood")]
[JsonDerivedType(typeof(Sand), "Sand")]
[JsonDerivedType(typeof(MagicMineral), "MagicMineral")]
//
[JsonDerivedType(typeof(HealthElixir), "HealthElixir")]
[JsonDerivedType(typeof(HealingElixir), "HealingElixir")]
[JsonDerivedType(typeof(PowerElixir), "PowerElixir")]
[JsonDerivedType(typeof(Poison), "Poison")]
[JsonDerivedType(typeof(Antidote), "Antidote")]
//
[JsonDerivedType(typeof(Sword), "Sword")]
[JsonDerivedType(typeof(Knife), "Knife")]
[JsonDerivedType(typeof(BigSword), "BigSword")]
[JsonDerivedType(typeof(Shield), "Shield")]
//
[JsonDerivedType(typeof(StrongWeapon), "StrongWeapon")]
[JsonDerivedType(typeof(LuckyWeapon), "LuckyWeapon")]
[JsonDerivedType(typeof(DefensiveWeapon), "DefensiveWeapon")]
[JsonDerivedType(typeof(OffensiveWeapon), "OffensiveWeapon")]
[JsonDerivedType(typeof(SlowWeapon), "SlowWeapon")]
[JsonDerivedType(typeof(FastWeapon), "FastWeapon")]
public interface IPickupable : IItem
{
    public bool Apply(Body b, string bpName);
}

[JsonPolymorphic]
[JsonDerivedType(typeof(HealthElixir), "HealthElixir")]
[JsonDerivedType(typeof(HealingElixir), "HealingElixir")]
[JsonDerivedType(typeof(PowerElixir), "PowerElixir")]
[JsonDerivedType(typeof(Poison), "Poison")]
[JsonDerivedType(typeof(Antidote), "Antidote")]
//
[JsonDerivedType(typeof(Sword), "Sword")]
[JsonDerivedType(typeof(Knife), "Knife")]
[JsonDerivedType(typeof(BigSword), "BigSword")]
[JsonDerivedType(typeof(Shield), "Shield")]
//
[JsonDerivedType(typeof(StrongWeapon), "StrongWeapon")]
[JsonDerivedType(typeof(LuckyWeapon), "LuckyWeapon")]
[JsonDerivedType(typeof(DefensiveWeapon), "DefensiveWeapon")]
[JsonDerivedType(typeof(OffensiveWeapon), "OffensiveWeapon")]
[JsonDerivedType(typeof(SlowWeapon), "SlowWeapon")]
[JsonDerivedType(typeof(FastWeapon), "FastWeapon")]
public interface IUsable : IPickupable
{
    public int Damage { get; set; }
    public bool IsTwoHanded { get; set; }
    public void AcceptAttack(Fight playerEnemyFight);
    public void Use(Player p, string bpName);
    public void AssignAttributes(Dictionary<string, int> attributes);
}
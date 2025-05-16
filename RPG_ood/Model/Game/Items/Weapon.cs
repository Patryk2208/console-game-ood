using System.Text.Json.Serialization;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Effects;
using RPG_ood.Model.Game.Attack;
using RPG_ood.Model.Game.Beings;
using RPG_ood.View;

namespace RPG_ood.Model.Game.Items;

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
public interface IWeapon : IItem, IUsable, IPickupable
{
    void IItem.Interact(Player p)
    {
        p.Eq.AddItemToEq(this);
    }
    bool IPickupable.Apply(Body b, string bpName)
    {
        if (IsTwoHanded)
        {
            if (!b.BodyParts["LeftHand"].IsUsed && !b.BodyParts["RightHand"].IsUsed)
            {
                b.BodyParts["LeftHand"].PutOn(this);
                b.BodyParts["RightHand"].PutOn(this);
                return true;
            }
        }
        else
        {
            b.BodyParts[bpName].PutOn(this);
            return true;
        }
        return false;
    }
    string IItem.PrintName() => $"{Name} [{Damage}]";
}

[Serializable]
public abstract class Weapon : IWeapon
{
    public string Name { get; set; }
    public Position Pos { get; set; }
    public int Damage { get; set; }
    public bool IsTwoHanded { get; set; }
    
    public Weapon() {}

    [JsonConstructor]
    public Weapon(string name, int damage, Position pos, bool isTwoHanded)
    {
        Name = name;
        Damage = damage;
        Pos = pos;
        IsTwoHanded = isTwoHanded;
    }
    
    public void AssignAttributes(Dictionary<string, int> attributes) {}
    public void Use(Player p, string bpName) {}
    public abstract void AcceptAttack(Fight playerEnemyFight);
    
    public abstract void AcceptView(IViewGenerator generator);
}

public abstract class HeavyWeapon : Weapon
{
    public HeavyWeapon() : base() {}
    [JsonConstructor]
    public HeavyWeapon(string name, int damage, Position pos, bool isTwoHanded) : 
        base(name, damage, pos, isTwoHanded) {}
    public override void AcceptAttack(Fight playerEnemyFight)
    {
        playerEnemyFight.VisitHeavyWeapon(this);
    }
}

public abstract class LightWeapon : Weapon
{
    public LightWeapon() : base() {}
    [JsonConstructor]
    public LightWeapon(string name, int damage, Position pos, bool isTwoHanded) : 
        base(name, damage, pos, isTwoHanded) {}
    public override void AcceptAttack(Fight playerEnemyFight)
    {
        playerEnemyFight.VisitLightWeapon(this); 
    }
}

public abstract class MagicalWeapon : Weapon
{
    public MagicalWeapon() : base() {}
    [JsonConstructor]
    public MagicalWeapon(string name, int damage, Position pos, bool isTwoHanded) : 
        base(name, damage, pos, isTwoHanded) {}
    public override void AcceptAttack(Fight playerEnemyFight)
    {
        playerEnemyFight.VisitMagicalWeapon(this);
    }
}

public class Sword : HeavyWeapon
{
    public Sword()
    {
        Name = "Sword";
        Damage = 100;
        IsTwoHanded = false;
    }
    
    [JsonConstructor]
    public Sword(string name, int damage, Position pos, bool isTwoHanded) : base(name, damage, pos, isTwoHanded) {}

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitSword(this);
    }
}

public class Knife : LightWeapon
{
    public Knife()
    {
        Name = "Knife";
        Damage = 50;
        IsTwoHanded = false;
    }

    [JsonConstructor]
    public Knife(string name, int damage, Position pos, bool isTwoHanded) : base(name, damage, pos, isTwoHanded) {}
    
    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitKnife(this);
    }
}

public class BigSword : HeavyWeapon
{
    public BigSword()
    {
        Name = "Big Sword";
        Damage = 150;
        IsTwoHanded = true;
    }
    
    [JsonConstructor]
    public BigSword(string name, int damage, Position pos, bool isTwoHanded) : base(name, damage, pos, isTwoHanded) {}

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitBigSword(this);
    }
}

public class Shield : HeavyWeapon
{
    public Shield()
    {
        Name = "Shield";
        Damage = 100;
        IsTwoHanded = false;
    }
    
    [JsonConstructor]
    public Shield(string name, int damage, Position pos, bool isTwoHanded) : base(name, damage, pos, isTwoHanded) {}

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitShield(this);
    }
}
using Project_oob.Beings;
using Project_oob.Effects;

namespace Project_oob.Items;

public abstract class Weapon : Item, IPickupable, IUsable, IDecorable<Weapon>
{
    public int Damage { get; protected set; }

    public override void Interact(Player p, Item? item)
    {
        p.Eq.AddItemToEq(item ?? this);
    }
    public new Weapon Decorated { get; set; } = null!;
    public override void AssignAttributes(Dictionary<string, int> attributes) {}

    public abstract override bool Apply(Body b, string bpName, Item? item);
    public override string PrintName() => $"{Name} [{Damage}]";
}

public abstract class TwoHandedWeapon : Weapon
{
    public override bool Apply(Body b, string bpName, Item? item)
    {
        if (!b.BodyParts["LeftHand"].IsUsed && !b.BodyParts["RightHand"].IsUsed)
        {
            b.BodyParts["LeftHand"].PutOn(item ?? this);
            b.BodyParts["RightHand"].PutOn(item ?? this);
            return true;
        }
        return false;
    }
}


public abstract class OneHandedWeapon : Weapon
{
    public override bool Apply(Body b, string bpName, Item? item)
    {
        b.BodyParts[bpName].PutOn(item ?? this);
        return true;
    }
}

public class Sword : OneHandedWeapon
{
    public Sword()
    {
        Name = "Sword";
        Color = 37;
        Damage = 100;
    }
    
    public override string ToString()
    {
        return "S";
    }
}

public class Knife : OneHandedWeapon
{
    public Knife()
    {
        Name = "Knife";
        Color = 37;
        Damage = 50;
    }

    public override string ToString()
    {
        return "K";
    }
}

public class BigSword : TwoHandedWeapon
{
    public BigSword()
    {
        Name = "Big Sword";
        Color = 37;
        Damage = 150;
    }
    public override string ToString()
    {
        return "B";
    }
}

public class Shield : OneHandedWeapon
{
    public Shield()
    {
        Name = "Shield";
        Color = 31;
        Damage = 100;
    }

    public override string ToString()
    {
        return "D";
    }
}
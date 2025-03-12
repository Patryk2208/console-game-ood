using Project_oob.Beings;
using Project_oob.Effects;

namespace Project_oob.Items;

public abstract class Weapon : Item, IPickupable, IUsable, IDecorable<Weapon>
{
    public int Damage { get; protected set; }
    public bool IsTwoHanded { get; protected set; }

    public override void Interact(Player p)
    {
        p.Eq.AddItemToEq(this);
    }
    public new Weapon Decorated { get; set; } = null!;
    public override void AssignAttributes(Dictionary<string, int> attributes) {}

    public override bool Apply(Body b, string bpName)
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
    
    public override string PrintName() => $"{Name} [{Damage}]";
}

/*public abstract class TwoHandedWeapon : Weapon
{
    public override bool Apply(Body b, string bpName/*, Item? item#1#)
    {
        if (!b.BodyParts["LeftHand"].IsUsed && !b.BodyParts["RightHand"].IsUsed)
        {
            /*b.BodyParts["LeftHand"].PutOn(item ?? this);
            b.BodyParts["RightHand"].PutOn(item ?? this);#1#
            b.BodyParts["LeftHand"].PutOn(this);
            b.BodyParts["RightHand"].PutOn(this);
            return true;
        }
        return false;
    }
}


public abstract class OneHandedWeapon : Weapon
{
    public override bool Apply(Body b, string bpName/*, Item? item#1#)
    {
        //b.BodyParts[bpName].PutOn(item ?? this);
        b.BodyParts[bpName].PutOn(this);
        return true;
    }
}*/

public class Sword : Weapon
{
    public Sword()
    {
        Name = "Sword";
        Color = 37;
        Damage = 100;
        IsTwoHanded = false;
    }
    
    public override string ToString()
    {
        return "S";
    }
}

public class Knife : Weapon
{
    public Knife()
    {
        Name = "Knife";
        Color = 37;
        Damage = 50;
        IsTwoHanded = false;
    }

    public override string ToString()
    {
        return "K";
    }
}

public class BigSword : Weapon
{
    public BigSword()
    {
        Name = "Big Sword";
        Color = 37;
        Damage = 150;
        IsTwoHanded = true;
    }
    public override string ToString()
    {
        return "B";
    }
}

public class Shield : Weapon
{
    public Shield()
    {
        Name = "Shield";
        Color = 31;
        Damage = 100;
        IsTwoHanded = false;
    }

    public override string ToString()
    {
        return "D";
    }
}
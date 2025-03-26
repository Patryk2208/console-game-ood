using RPG_ood.Effects;
using RPG_ood.Beings;

namespace RPG_ood.Items;

public interface IWeapon : IItem, IUsable, IPickupable
{
    public int Damage { get; set; }
    public bool IsTwoHanded { get; set; }
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
public abstract class Weapon : IWeapon
{
    public string Name { get; set; }
    public Position Pos { get; set; }
    public int Color { get; set; }
    public int Damage { get; set; }
    public bool IsTwoHanded { get; set; }
    public void AssignAttributes(Dictionary<string, int> attributes) {}
}

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
        return "s";
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
        return "k";
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
        return "b";
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
        return "d";
    }
}
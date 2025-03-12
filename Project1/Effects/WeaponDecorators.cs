using Project_oob.Beings;
using Project_oob.Items;

namespace Project_oob.Effects;

public abstract class WeaponDecorator : Weapon
{
    protected WeaponDecorator(Weapon item)
    {
        Decorated = item;
        Pos = Decorated.Pos;
        Color = Decorated.Color;
        Damage = Decorated.Damage;
    }

    public abstract override void AssignAttributes(Dictionary<string, int> attributes);
    public override void Interact(Player p, Item? item)
    {
        Decorated.Interact(p, item ?? this);
    }
    public override string ToString()
    {
        return Decorated.ToString();
    }
    public override bool Apply(Body b, string bpName, Item? item)
    {
        return Decorated.Apply(b, bpName, item ?? this);
    }
}


public class StrongWeapon : WeaponDecorator
{
    public StrongWeapon(Weapon item) : base(item)
    {
        Name = "(Strong) " + Decorated.Name;
        Damage = Decorated.Damage + 30;
    }

    public override void AssignAttributes(Dictionary<string, int> attributes)
    {
        Decorated.AssignAttributes(attributes);
        if (attributes.TryGetValue("Power", out int power))
        {
            power += 10;
            attributes["Power"] = power;
        }
    }
}

public class LuckyWeapon : WeaponDecorator
{
    public LuckyWeapon(Weapon item) : base(item)
    {
        Name = "(Lucky) " + Decorated.Name;
    }

    public override void AssignAttributes(Dictionary<string, int> attributes)
    {
        Decorated.AssignAttributes(attributes);
        if (attributes.TryGetValue("Luck", out int luck))
        {
            luck += 10;
            attributes["Luck"] = luck;
        }
    }
}

public class DefensiveWeapon : WeaponDecorator
{
    public DefensiveWeapon(Weapon item) : base(item)
    {
        Name = "(Defensive) " + Decorated.Name;
        Damage = Decorated.Damage - 20;
    }

    public override void AssignAttributes(Dictionary<string, int> attributes)
    {
        Decorated.AssignAttributes(attributes);
        if (attributes.TryGetValue("Wisdom", out int wisdom))
        { 
            wisdom += 10;
            attributes["Wisdom"] = wisdom;
        }
        if (attributes.TryGetValue("Agility", out int agility))
        {
            agility += 10;
            attributes["Agility"] = agility;
        }
        if (attributes.TryGetValue("Health", out int Health))
        {
            Health += 10;
            attributes["Health"] = Health;
        }
    }
}

public class OffensiveWeapon : WeaponDecorator
{
    public OffensiveWeapon(Weapon item) : base(item)
    {
        Name = "(Offensive) " + Decorated.Name;
        Damage = Decorated.Damage + 10;
    }

    public override void AssignAttributes(Dictionary<string, int> attributes)
    {
        Decorated.AssignAttributes(attributes);
        if (attributes.TryGetValue("Power", out int Power))
        {
            Power += 10;
            attributes["Power"] = Power;
        }
        if (attributes.TryGetValue("Agility", out int Agility))
        {
            Agility -= 10;
            attributes["Agility"] = Agility;
        }
        if (attributes.TryGetValue("Aggression", out int Aggression))
        {
            Aggression += 10;
            attributes["Aggression"] = Aggression;
        }
    }
}

public class HeavyWeapon : WeaponDecorator
{
    public HeavyWeapon(Weapon item) : base(item)
    {
        Name = "(Heavy) " + Decorated.Name;
        Damage = Decorated.Damage + 20;
    }

    public override void AssignAttributes(Dictionary<string, int> attributes)
    {
        Decorated.AssignAttributes(attributes);
        if (attributes.TryGetValue("Agility", out int Agility))
        {
            Agility -= 10;
            attributes["Agility"] = Agility;
        }
        if (attributes.TryGetValue("Power", out int Power))
        {
            Power += 10;
            attributes["Power"] = Power;
        }
    }
}

public class LightWeapon : WeaponDecorator
{
    public LightWeapon(Weapon item) : base(item)
    {
        Name = "(Light) " + Decorated.Name;
        Damage = Decorated.Damage - 30;
    }

    public override void AssignAttributes(Dictionary<string, int> attributes)
    {
        Decorated.AssignAttributes(attributes);
        if (attributes.TryGetValue("Agility", out int Agility))
        {
            Agility += 10;
            attributes["Agility"] = Agility;
        }

        if (attributes.TryGetValue("Power", out int Power))
        {
            Power -= 10;
            attributes["Power"] = Power;
        }
    }
}

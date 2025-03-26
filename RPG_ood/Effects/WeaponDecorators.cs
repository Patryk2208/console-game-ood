using RPG_ood.Beings;
using RPG_ood.Items;

namespace RPG_ood.Effects;

public abstract class WeaponDecorator : IWeapon
{
    public abstract string Name { get; set; }
    public abstract int Damage { get; set; }
    public new IWeapon Decorated { get; set; }
    protected WeaponDecorator(IWeapon item)
    {
        Decorated = item;
    }
    public Position Pos
    {
        get => Decorated.Pos;
        set => Decorated.Pos = value;
    }
    public int Color
    {
        get => Decorated.Color;
        set => Decorated.Color = value;
    }
    public bool IsTwoHanded
    {
        get => Decorated.IsTwoHanded;
        set => Decorated.IsTwoHanded = value;
    }
    public abstract void AssignAttributes(Dictionary<string, int> attributes);
    public override string ToString()
    {
        return Decorated.ToString();
    }
}


public class StrongWeapon : WeaponDecorator
{
    public StrongWeapon(IWeapon item) : base(item) {}
    public override string Name
    {
        get => "(Strong) " + Decorated.Name;
        set => Decorated.Name = value;
    }
    public override int Damage
    {
        get => Decorated.Damage + 30;
        set => Decorated.Damage = value;
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
    public LuckyWeapon(IWeapon item) : base(item) {}
    public override string Name
    {
        get => "(Lucky) " + Decorated.Name;
        set => Decorated.Name = value;
    }
    public override int Damage
    {
        get => Decorated.Damage;
        set => Decorated.Damage = value;
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
    public DefensiveWeapon(IWeapon item) : base(item) {}
    public override string Name
    {
        get => "(Defensive) " + Decorated.Name;
        set => Decorated.Name = value;
    }
    public override int Damage
    {
        get => Decorated.Damage - 20;
        set => Decorated.Damage = value;
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
    public OffensiveWeapon(IWeapon item) : base(item) {}
    public override string Name
    {
        get => "(Offensive) " + Decorated.Name;
        set => Decorated.Name = value;
    }
    public override int Damage
    {
        get => Decorated.Damage + 10;
        set => Decorated.Damage = value;
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
    public HeavyWeapon(IWeapon item) : base(item) {}
    public override string Name
    {
        get => "(Heavy) " + Decorated.Name;
        set => Decorated.Name = value;
    }
    public override int Damage
    {
        get => Decorated.Damage + 20;
        set => Decorated.Damage = value;
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
    public LightWeapon(IWeapon item) : base(item) {}
    public override string Name
    {
        get => "(Light) " + Decorated.Name;
        set => Decorated.Name = value;
    }
    public override int Damage
    {
        get => Decorated.Damage - 30;
        set => Decorated.Damage = value;
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

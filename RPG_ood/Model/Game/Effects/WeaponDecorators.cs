using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game.Attack;
using RPG_ood.Model.Game.Beings;
using RPG_ood.Model.Game.Items;
using RPG_ood.Model.Items;
using RPG_ood.View;

namespace RPG_ood.Model.Effects;

[Serializable]
[JsonConverter(typeof(WeaponDecoratorJsonConverter))]
public abstract class WeaponDecorator : IWeapon
{
    public abstract string Name { get; set; }
    public abstract int Damage { get; set; }
    public IWeapon Decorated { get; set; }
    protected WeaponDecorator(IWeapon item)
    {
        Decorated = item;
    }

    [JsonConstructor]
    public WeaponDecorator(string name, int damage, IWeapon decorated, Position pos, bool isTwoHanded)
    {
        Decorated = decorated;
        Name = name;
        Damage = damage;
        Pos = pos;
        IsTwoHanded = isTwoHanded;
    }
    public Position Pos
    {
        get => Decorated.Pos;
        set => Decorated.Pos = value;
    }
    public bool IsTwoHanded
    {
        get => Decorated.IsTwoHanded;
        set => Decorated.IsTwoHanded = value;
    }

    public void AcceptAttack(Fight playerEnemyFight)
    {
        Decorated.AcceptAttack(playerEnemyFight);
    }

    public void Use(Player p, string bpName) {}

    public abstract void AssignAttributes(Dictionary<string, int> attributes);

    public void AcceptView(IViewGenerator generator)
    {
        Decorated.AcceptView(generator);
    }
}

public class WeaponDecoratorJsonConverter : JsonConverter<WeaponDecorator>
{
    public override WeaponDecorator? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            options.Converters.Remove(this);
            WeaponDecorator weaponDecorator = root.Deserialize<WeaponDecorator>(options);
            options.Converters.Add(this);
            /*string name = root.GetProperty("Name").GetString();
            int damage = root.GetProperty("Damage").GetInt32();
            Position pos = root.GetProperty("Pos").Deserialize<Position>(options);
            bool isTwoHanded = root.GetProperty("TwoHanded").GetBoolean();*/
            IWeapon decorated;
            JsonElement internalRoot = root.GetProperty("Decorated");
            JsonElement decoratedCheck;
            try
            {
                decoratedCheck = internalRoot.GetProperty("Decorated");
                decorated = (IWeapon)internalRoot.Deserialize<WeaponDecorator>(options);
            }
            catch (KeyNotFoundException)
            {
                decorated = (IWeapon)internalRoot.Deserialize<Weapon>(options);
            }
            weaponDecorator.Decorated = decorated;

            return weaponDecorator;
        }
    }

    public override void Write(Utf8JsonWriter writer, WeaponDecorator value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Name");
        writer.WriteStringValue(value.Name);
        writer.WritePropertyName("Damage");
        writer.WriteNumberValue(value.Damage);
        writer.WritePropertyName("Decorated");
        JsonSerializer.Serialize(writer, value.Decorated, options);
        writer.WritePropertyName("Pos");
        JsonSerializer.Serialize(writer, value.Pos, options);
        writer.WritePropertyName("IsTwoHanded");
        writer.WriteBooleanValue(value.IsTwoHanded);
        writer.WriteEndObject();
    }
}


public class StrongWeapon : WeaponDecorator
{
    public StrongWeapon(IWeapon item) : base(item) {}

    [JsonConstructor]
    public StrongWeapon(string name, int damage, IWeapon decorated, Position pos, bool isTwoHanded) : base(name,
        damage, decorated, pos, isTwoHanded) {}
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
    
    [JsonConstructor]
    public LuckyWeapon(string name, int damage, IWeapon decorated, Position pos, bool isTwoHanded) : base(name,
        damage, decorated, pos, isTwoHanded) {}
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
    
    [JsonConstructor]
    public DefensiveWeapon(string name, int damage, IWeapon decorated, Position pos, bool isTwoHanded) : base(name,
        damage, decorated, pos, isTwoHanded) {}
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
    
    [JsonConstructor]
    public OffensiveWeapon(string name, int damage, IWeapon decorated, Position pos, bool isTwoHanded) : base(name,
        damage, decorated, pos, isTwoHanded) {}
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

public class SlowWeapon : WeaponDecorator
{
    public SlowWeapon(IWeapon item) : base(item) {}
    
    [JsonConstructor]
    public SlowWeapon(string name, int damage, IWeapon decorated, Position pos, bool isTwoHanded) : base(name,
        damage, decorated, pos, isTwoHanded) {}
    public override string Name
    {
        get => "(Slow) " + Decorated.Name;
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

public class FastWeapon : WeaponDecorator
{
    public FastWeapon(IWeapon item) : base(item) {}
    [JsonConstructor]
    public FastWeapon(string name, int damage, IWeapon decorated, Position pos, bool isTwoHanded) : base(name,
        damage, decorated, pos, isTwoHanded) {}
    public override string Name
    {
        get => "(Fast) " + Decorated.Name;
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
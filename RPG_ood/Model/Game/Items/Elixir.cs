using System.Text.Json.Serialization;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game;
using RPG_ood.Model.Game.Attack;
using RPG_ood.Model.Game.Beings;
using RPG_ood.Model.Game.GameState;
using RPG_ood.Model.Game.Items;
using RPG_ood.View;

namespace RPG_ood.Model.Items;

public interface IElixir : IUsable, IPickupable, IItem, IObserver
{
    
}

public abstract class Elixir : IElixir
{
    public int Damage { get; set; } = 0;
    public bool IsTwoHanded { get; set; } = false;
    public Position Pos { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    protected int EffectDurationInMoments { get; set; }
    [JsonIgnore]
    protected int MomentsPassed { get; set; } = 0;
    [JsonIgnore]
    protected int MomentInterval { get; set; }
    
    public Elixir() {}

    [JsonConstructor]
    public Elixir(string name, int damage, Position pos, bool isTwoHanded)
    {
        Name = name;
        Damage = damage;
        Pos = pos;
        IsTwoHanded = isTwoHanded;
    }
    
    public void Interact(Player p)
    {
        p.Eq.AddItemToEq(this);
    }
    public string PrintName()
    {
        return $"{Name}";
    }
    public bool Apply(Body b, string bpName)
    {
        b.BodyParts[bpName].PutOn(this);
        return true;
    }
    public abstract void Use(Player p, string bpName);
    public void AssignAttributes(Dictionary<string, int> attributes) {}
    public abstract void Update(GameState? state, long id);
    public void AcceptAttack(Fight playerEnemyFight)
    {
        playerEnemyFight.VisitOtherUsable(this);
    }
    
    public abstract void AcceptView(IViewGenerator generator);
}

public class HealingElixir : Elixir
{
    [JsonIgnore]
    private int BaseHealth { get; set; }
    [JsonIgnore]
    private int Increment { get; set; }
    public HealingElixir()
    {
        Name = "Healing Elixir";
    }

    [JsonConstructor]
    public HealingElixir(string name, int damage, Position pos, bool isTwoHanded) : 
        base(name, damage, pos, isTwoHanded) {}
    public override void Use(Player p, string bpName)
    {
        var used = p.Bd.BodyParts[bpName].TakeOff();
        used.Pos = used.Pos with { X = -1, Y = -1 };
        Increment = 2;
        EffectDurationInMoments = 50;
        MomentInterval = 5;
        p.MomentChangedEvent.AddObserver(Name, this);
    }

    public override void Update(GameState? state, long id)
    {
        ++MomentsPassed;
        if(state == null || !state.Players.TryGetValue(id, out var p)) return;
        if (MomentsPassed >= EffectDurationInMoments)
        {
            p.MomentChangedEvent.RemoveObserver(Name, this);
        }
        else if (MomentsPassed % MomentInterval == 0)
        {
            p.ReceiveDamage(-Increment);
        }
    }

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitHealingElixir(this);
    }
}

public class PowerElixir : Elixir
{
    private int BasePower { get; set; }
    private int Increment { get; set; }
    public PowerElixir()
    {
        Name = "Power Elixir";
    }
    
    [JsonConstructor]
    public PowerElixir(string name, int damage, Position pos, bool isTwoHanded) : 
        base(name, damage, pos, isTwoHanded) {}
    public override void Use(Player p, string bpName)
    {
        var used = p.Bd.BodyParts[bpName].TakeOff();
        used.Pos = used.Pos with { X = -1, Y = -1 };
        EffectDurationInMoments = 100;
        MomentInterval = 5;
        Increment = 15;
        BasePower = p.Attr["Power"].Value;
        p.Attr["Power"].Value = BasePower + Increment;
        p.MomentChangedEvent.AddObserver(Name, this);
    }
    public override void Update(GameState? state, long id)
    {
        ++MomentsPassed;
        if(state == null || !state.Players.TryGetValue(id, out var p)) return;
        if (MomentsPassed >= EffectDurationInMoments)
        {
            p.Attr["Power"].Value = BasePower;
            p.MomentChangedEvent.RemoveObserver(Name, this);
        }
        else if (MomentsPassed % MomentInterval == 0)
        {
            BasePower = p.Attr["Power"].Value - Increment;
            p.Attr["Power"].Value = BasePower + Increment;
        }
    }

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitPowerElixir(this);
    }
}
public class Poison : Elixir
{
    private int BaseHealth { get; set; }
    private int Increment { get; set; }
    public Poison()
    {
        Name = "Poison";
    }
    
    [JsonConstructor]
    public Poison(string name, int damage, Position pos, bool isTwoHanded) : 
        base(name, damage, pos, isTwoHanded) {}
    public override void Use(Player p, string bpName)
    {
        var used = p.Bd.BodyParts[bpName].TakeOff();
        used.Pos = used.Pos with { X = -1, Y = -1 };
        Increment = -1;
        EffectDurationInMoments = 60;
        MomentInterval = 2;
        p.MomentChangedEvent.AddObserver(Name, this);
    }

    public override void Update(GameState? state, long id)
    {
        ++MomentsPassed;
        if(state == null || !state.Players.TryGetValue(id, out var p)) return;
        if (MomentsPassed >= EffectDurationInMoments)
        {
            p.MomentChangedEvent.RemoveObserver(Name, this);
        }
        else if (MomentsPassed % MomentInterval == 0)
        {
            p.ReceiveDamage(-Increment);
        }
    }

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitPoison(this);
    }
}
public class Antidote : Elixir
{
    private int BaseHealth { get; set; }
    private int Increment { get; set; }
    public Antidote()
    {
        Name = "Antidote";
    }
    
    [JsonConstructor]
    public Antidote(string name, int damage, Position pos, bool isTwoHanded) : 
        base(name, damage, pos, isTwoHanded) {}
    public override void Use(Player p, string bpName)
    {
        var used = p.Bd.BodyParts[bpName].TakeOff();
        used.Pos = used.Pos with { X = -1, Y = -1 };
        p.MomentChangedEvent.RemoveObserversByName("Poison");
    }

    public override void Update(GameState? state, long id) {}

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitAntidote(this);
    }
}

public class HealthElixir : Elixir
{
    private int BaseHealth { get; set; }
    private int Increment { get; set; }
    public HealthElixir()
    {
        Name = "Health Elixir";
    }
    [JsonConstructor]
    public HealthElixir(string name, int damage, Position pos, bool isTwoHanded) : 
        base(name, damage, pos, isTwoHanded) {}
    public override void Use(Player p, string bpName)
    {
        var used = p.Bd.BodyParts[bpName].TakeOff();
        used.Pos = used.Pos with { X = -1, Y = -1 };
        BaseHealth = p.Attr["Health"].Value;
        Increment = 20;
        p.ReceiveDamage(-(BaseHealth + Increment));
        p.MomentChangedEvent.AddObserver(Name, this);
    }

    public override void Update(GameState? state, long id) {}

    public override void AcceptView(IViewGenerator generator)
    {
        generator.VisitHealthElixir(this);
    }
}
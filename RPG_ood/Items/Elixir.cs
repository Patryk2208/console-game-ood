using System.Xml;
using RPG_ood.Beings;
using RPG_ood.Effects;
using RPG_ood.Game;

namespace RPG_ood.Items;

public interface IElixir : IUsable, IPickupable, IItem, IObserver
{
    
}

public abstract class Elixir : IElixir
{
    public Position Pos { get; set; }
    public string Name { get; set; }
    public AnsiConsoleColor Color { get; set; }
    
    protected int EffectDurationInMoments { get; set; }
    protected int MomentsPassed { get; set; } = 0;
    protected int MomentInterval { get; set; }
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
    public abstract void Update(GameState? state);

    public override string ToString()
    {
        return "e";
    }
}

public class HealthElixir : Elixir
{
    private int BaseHealth { get; set; }
    private int Increment { get; set; }
    public HealthElixir()
    {
        Name = "Health Elixir";
        Color = AnsiConsoleColor.Cyan;
    }
    public override void Use(Player p, string bpName)
    {
        var used = p.Bd.BodyParts[bpName].TakeOff();
        used.Pos = used.Pos with { X = -1, Y = -1 };
        Increment = 2;
        EffectDurationInMoments = 50;
        MomentInterval = 5;
        p.MomentChangedEvent.AddObserver(Name, this);
    }

    public override void Update(GameState? state)
    {
        ++MomentsPassed;
        if(state == null) return;
        if (MomentsPassed >= EffectDurationInMoments)
        {
            state.Player.MomentChangedEvent.RemoveObserver(Name, this);
        }
        else if (MomentsPassed % MomentInterval == 0)
        {
            state.Player.Attr["Health"].Value += Increment;
        }
    }
}

public class PowerElixir : Elixir
{
    private int BasePower { get; set; }
    private int Increment { get; set; }
    public PowerElixir()
    {
        Name = "Power Elixir";
        Color = AnsiConsoleColor.BrightRed;
    }
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
    public override void Update(GameState? state)
    {
        ++MomentsPassed;
        if(state == null) return;
        if (MomentsPassed >= EffectDurationInMoments)
        {
            state.Player.Attr["Power"].Value = BasePower;
            state.Player.MomentChangedEvent.RemoveObserver(Name, this);
        }
        else if (MomentsPassed % MomentInterval == 0)
        {
            BasePower = state.Player.Attr["Power"].Value - Increment;
            state.Player.Attr["Power"].Value = BasePower + Increment;
        }
    }
    
}
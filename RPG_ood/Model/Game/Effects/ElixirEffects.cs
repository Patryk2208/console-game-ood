using RPG_ood.Model.Beings;
using RPG_ood.Model.Game;
using RPG_ood.Model.Game.Beings;
using RPG_ood.Model.Game.GameState;

namespace RPG_ood.Model.Effects;

//obsolete

/*public abstract class ElixirEffects : IObserver
{
    protected Player AffectedPlayer { get; set; }
    protected int EffectDurationInMoments { get; set; }
    protected int MomentsPassed { get; set; } = 0;
    protected int MomentInterval { get; set; }

    public ElixirEffects(Player player, int mdim, int mi)
    {
        AffectedPlayer = player;
        EffectDurationInMoments = mdim;
        MomentInterval = mi;
    }
    public abstract void Update(GameState? state);
}

public class HealthEffect : ElixirEffects
{
    private int BaseHealth { get; set; }
    private int Increment { get; set; }

    public HealthEffect(Player player, int mdim, int mi) : base(player, mdim, mi)
    {
        BaseHealth = AffectedPlayer.Attr["Health"].Value;
        Increment = 25;
        AffectedPlayer.Attr["Health"].Value = BaseHealth + Increment;
        AffectedPlayer.MomentChangedEvent.AddObserver("Health Effect", this);
    }
    public override void Update(GameState? state)
    {
        ++MomentsPassed;
        if (MomentsPassed >= EffectDurationInMoments)
        {
            AffectedPlayer.MomentChangedEvent.RemoveObserver("Health Effect", this);
        }
    }
}

public class HealingEffect : ElixirEffects
{
    private int Increment { get; set; }
    public HealingEffect(Player player, int mdim, int mi) : base(player, mdim, mi)
    {
        Increment = 2;
        AffectedPlayer.MomentChangedEvent.AddObserver("Healing Effect", this);
    }

    public override void Update(GameState? state)
    {
        ++MomentsPassed;
        if (MomentsPassed >= EffectDurationInMoments)
        {
            AffectedPlayer.MomentChangedEvent.RemoveObserver("Healing Effect", this);
        }
        else if (MomentsPassed % MomentInterval == 0)
        {
            AffectedPlayer.Attr["Health"].Value += Increment;
        }
    }
}

public class PowerEffect : ElixirEffects
{
    private int BasePower { get; set; }
    private int Increment { get; set; }
    public PowerEffect(Player player, int mdim, int mi) : base(player, mdim, mi)
    {
        BasePower = AffectedPlayer.Attr["Power"].Value;
        Increment = 15;
        AffectedPlayer.Attr["Power"].Value += Increment;
        AffectedPlayer.MomentChangedEvent.AddObserver("Power Effect", this);
    }

    public override void Update(GameState? state)
    {
        ++MomentsPassed;
        if (MomentsPassed >= EffectDurationInMoments)
        {
            AffectedPlayer.Attr["Power"].Value = BasePower;
            AffectedPlayer.MomentChangedEvent.RemoveObserver("Power Effect", this);
        }
        else if (MomentsPassed % MomentInterval == 0)
        {
            BasePower = AffectedPlayer.Attr["Power"].Value - Increment;
            AffectedPlayer.Attr["Power"].Value = BasePower + Increment;
        }
    }
}*/
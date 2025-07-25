using Model.Game.Beings;
using Model.Game.Items;

namespace Model.Game.Attack;

public abstract class PlayersFight(Player attacker, Player defender, IUsable weapon) : Fight
{
    protected Player Attacker { get; set; } = attacker;
    protected Player Defender { get; set; } = defender;
    protected IUsable Weapon { get; set; } = weapon;
    protected int AttackDamage { get; set; } = 0;
    
    public abstract override void VisitHeavyWeapon(HeavyWeapon usedWeapon);
    public abstract override void VisitLightWeapon(LightWeapon usedWeapon);
    public abstract override void VisitMagicalWeapon(MagicalWeapon usedWeapon);
    public abstract override void VisitOtherUsable(IUsable usedWeapon);
    
    public override void Attack()
    {
        Defender.ReceiveDamage(AttackDamage);
    }

}

//TODO

public class NormalPlayersAttack(Player attacker, Player defender, IUsable weapon) : PlayersFight(attacker, defender, weapon)
{
    public override void VisitHeavyWeapon(HeavyWeapon usedWeapon)
    {
        AttackDamage = 5;
    }

    public override void VisitLightWeapon(LightWeapon usedWeapon)
    {
        AttackDamage = 5;
    }

    public override void VisitMagicalWeapon(MagicalWeapon usedWeapon)
    {
        AttackDamage = 5;
    }

    public override void VisitOtherUsable(IUsable usedWeapon)
    {
        AttackDamage = 0;
    }
}

public class SneakPlayersAttack(Player attacker, Player defender, IUsable weapon) : PlayersFight(attacker, defender, weapon)
{
    public override void VisitHeavyWeapon(HeavyWeapon usedWeapon)
    {
        AttackDamage = 5;
    }

    public override void VisitLightWeapon(LightWeapon usedWeapon)
    {
        AttackDamage = 5;
    }

    public override void VisitMagicalWeapon(MagicalWeapon usedWeapon)
    {
        AttackDamage = 5;
    }

    public override void VisitOtherUsable(IUsable usedWeapon)
    {
        AttackDamage = 0;
    }
}

public class MagicPlayersAttack(Player attacker, Player defender, IUsable weapon) : PlayersFight(attacker, defender, weapon)
{
    public override void VisitHeavyWeapon(HeavyWeapon usedWeapon)
    {
        AttackDamage = 5;
    }

    public override void VisitLightWeapon(LightWeapon usedWeapon)
    {
        AttackDamage = 5;
    }

    public override void VisitMagicalWeapon(MagicalWeapon usedWeapon)
    {
        AttackDamage = 5;
    }

    public override void VisitOtherUsable(IUsable usedWeapon)
    {
        AttackDamage = 0;
    }
}
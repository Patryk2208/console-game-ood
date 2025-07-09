using Model.Game.Beings;
using Model.Game.Items;

namespace Model.Game.Attack;

public abstract class PlayerEnemyFight(Player attacker, IEnemy defender, IUsable weapon) : Fight
{
    protected Player Attacker { get; set; } = attacker;
    protected IEnemy Defender { get; set; } = defender;
    protected IUsable Weapon { get; set; } = weapon;
    protected int AttackDamage { get; set; } = 0;
    protected int CounterAttackDamage { get; set; } = defender.Damage;

    public abstract override void VisitHeavyWeapon(HeavyWeapon usedWeapon);
    public abstract override void VisitLightWeapon(LightWeapon usedWeapon);
    public abstract override void VisitMagicalWeapon(MagicalWeapon usedWeapon);
    public abstract override void VisitOtherUsable(IUsable usedWeapon);

    public override void Attack()
    {
        Defender.ReceiveDamage(AttackDamage);
    }
}

public class NormalPlayerEnemyAttack(Player attacker, IEnemy defender, IUsable weapon) : PlayerEnemyFight(attacker, defender, weapon)
{
    public override void VisitHeavyWeapon(HeavyWeapon usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Power"].Value / Attacker.Attr["Power"].MaxValue) + 0.5));
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Aggression"].Value / Attacker.Attr["Aggression"].MaxValue) + 0.5));
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }

    public override void VisitLightWeapon(LightWeapon usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Agility"].Value / Attacker.Attr["Agility"].MaxValue) + 0.5));
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Luck"].Value / Attacker.Attr["Luck"].MaxValue) + 0.5));
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }

    public override void VisitMagicalWeapon(MagicalWeapon usedWeapon)
    {
        AttackDamage = 1;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }

    public override void VisitOtherUsable(IUsable usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }
}

public class SneakPlayerEnemyAttack(Player attacker, IEnemy defender, IUsable weapon) : PlayerEnemyFight(attacker, defender, weapon)
{
    public override void VisitHeavyWeapon(HeavyWeapon usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Power"].Value / Attacker.Attr["Power"].MaxValue) + 0.5));
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Aggression"].Value / Attacker.Attr["Aggression"].MaxValue) + 0.5));
        AttackDamage /= 2;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }

    public override void VisitLightWeapon(LightWeapon usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Agility"].Value / Attacker.Attr["Agility"].MaxValue) + 0.5));
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Luck"].Value / Attacker.Attr["Luck"].MaxValue) + 0.5));
        AttackDamage *= 2;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }

    public override void VisitMagicalWeapon(MagicalWeapon usedWeapon)
    {
        AttackDamage = 1;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }

    public override void VisitOtherUsable(IUsable usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }
}

public class MagicPlayerEnemyAttack(Player attacker, IEnemy defender, IUsable weapon) : PlayerEnemyFight(attacker, defender, weapon)
{
    public override void VisitHeavyWeapon(HeavyWeapon usedWeapon)
    {
        AttackDamage = 1;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }

    public override void VisitLightWeapon(LightWeapon usedWeapon)
    {
        AttackDamage = 1;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }

    public override void VisitMagicalWeapon(MagicalWeapon usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Wisdom"].Value / Attacker.Attr["Wisdom"].MaxValue) + 0.5));
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }

    public override void VisitOtherUsable(IUsable usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
    }
}
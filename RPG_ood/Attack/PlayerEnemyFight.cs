using RPG_ood.Beings;
using RPG_ood.Effects;
using RPG_ood.Items;

namespace RPG_ood.Attack;

public abstract class PlayerEnemyFight(Player attacker, IEnemy defender, IUsable weapon)
{
    public Player Attacker { get; set; } = attacker;
    public IEnemy Defender { get; set; } = defender;
    public IUsable Weapon { get; set; } = weapon;
    public int AttackDamage { get; set; } = 0;
    public int CounterAttackDamage { get; set; } = defender.Damage;

    public abstract void VisitHeavyWeapon(HeavyWeapon usedWeapon);
    public abstract void VisitLightWeapon(LightWeapon usedWeapon);
    public abstract void VisitMagicalWeapon(MagicalWeapon usedWeapon);
    public abstract void VisitOtherUsable(IUsable usedWeapon);

    public void Attack()
    {
        Defender.ReceiveDamage(AttackDamage);
    }

    public void CounterAttack()
    {
        Attacker.ReceiveDamage(CounterAttackDamage);
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
        
        var defence = (Attacker.Attr["Power"].Value + Attacker.Attr["Luck"].Value) / 2;
        defence += Attacker.Bd.GetArmor();
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
    }

    public override void VisitLightWeapon(LightWeapon usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Agility"].Value / Attacker.Attr["Agility"].MaxValue) + 0.5));
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Luck"].Value / Attacker.Attr["Luck"].MaxValue) + 0.5));
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
        
        var defence = (Attacker.Attr["Agility"].Value + Attacker.Attr["Luck"].Value) / 2;
        defence += Attacker.Bd.GetArmor();
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
    }

    public override void VisitMagicalWeapon(MagicalWeapon usedWeapon)
    {
        AttackDamage = 1;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
        
        var defence = (Attacker.Attr["Agility"].Value + Attacker.Attr["Luck"].Value) / 2;
        defence += Attacker.Bd.GetArmor();
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
    }

    public override void VisitOtherUsable(IUsable usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
        
        var defence = Attacker.Attr["Agility"].Value;
        defence += Attacker.Bd.GetArmor();
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
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
        
        var defence = Attacker.Attr["Power"].Value;
        defence += Attacker.Bd.GetArmor();
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
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

        var defence = Attacker.Attr["Agility"].Value;
        defence += Attacker.Bd.GetArmor();
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
    }

    public override void VisitMagicalWeapon(MagicalWeapon usedWeapon)
    {
        AttackDamage = 1;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);

        var defence = 0;
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
    }

    public override void VisitOtherUsable(IUsable usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);

        var defence = 0;
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
    }
}

public class MagicPlayerEnemyAttack(Player attacker, IEnemy defender, IUsable weapon) : PlayerEnemyFight(attacker, defender, weapon)
{
    public override void VisitHeavyWeapon(HeavyWeapon usedWeapon)
    {
        AttackDamage = 1;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);

        var defence = Attacker.Attr["Luck"].Value;
        defence += Attacker.Bd.GetArmor();
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
    }

    public override void VisitLightWeapon(LightWeapon usedWeapon)
    {
        AttackDamage = 1;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);

        var defence = Attacker.Attr["Luck"].Value;
        defence += Attacker.Bd.GetArmor();
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
    }

    public override void VisitMagicalWeapon(MagicalWeapon usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = (int)(AttackDamage * 
                            (((float)Attacker.Attr["Wisdom"].Value / Attacker.Attr["Wisdom"].MaxValue) + 0.5));
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);
        
        var defence = Attacker.Attr["Wisdom"].Value * 2;
        defence += Attacker.Bd.GetArmor();
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
    }

    public override void VisitOtherUsable(IUsable usedWeapon)
    {
        AttackDamage = Weapon.Damage;
        AttackDamage = Math.Max(AttackDamage - Defender.Armor, 0);

        var defence = Attacker.Attr["Luck"].Value;
        defence += Attacker.Bd.GetArmor();
        CounterAttackDamage = Math.Max(CounterAttackDamage - defence, 0);
    }
}
using RPG_ood.Model.Game.Beings;
using RPG_ood.Model.Game.Items;

namespace RPG_ood.Model.Game.Attack;

public class EnemyPlayerFight(IEnemy attacker, Player defender) : Fight
{
    private IEnemy Attacker { get; set; } = attacker;
    private Player Defender { get; set; } = defender;
    private int AttackDamage { get; set; }

    public override void Attack()
    {
        AttackDamage = Attacker.Damage;
        var playersDefence = Defender.Bd.GetArmor();
        playersDefence += (Defender.Attr["Power"].Value + Defender.Attr["Aggression"].Value +
                           Defender.Attr["Agility"].Value + Defender.Attr["Luck"].Value +
                           Defender.Attr["Wisdom"].Value) / 5;
        AttackDamage = Math.Max(AttackDamage - playersDefence, 0);
        Defender.ReceiveDamage(AttackDamage);
    }
    public override void VisitHeavyWeapon(HeavyWeapon usedWeapon)
    {
        throw new NotImplementedException();
    }

    public override void VisitLightWeapon(LightWeapon usedWeapon)
    {
        throw new NotImplementedException();
    }

    public override void VisitMagicalWeapon(MagicalWeapon usedWeapon)
    {
        throw new NotImplementedException();
    }

    public override void VisitOtherUsable(IUsable usedWeapon)
    {
        throw new NotImplementedException();
    }
}
using RPG_ood.Model.Game.Items;

namespace RPG_ood.Model.Game.Attack;

public abstract class Fight
{
    public abstract void VisitHeavyWeapon(HeavyWeapon usedWeapon);
    public abstract void VisitLightWeapon(LightWeapon usedWeapon);
    public abstract void VisitMagicalWeapon(MagicalWeapon usedWeapon);
    public abstract void VisitOtherUsable(IUsable usedWeapon);

    public abstract void Attack();

    public abstract void CounterAttack();
}
using Model.Game.Items;

namespace Model.Game.Attack;

public abstract class Fight
{
    public abstract void VisitHeavyWeapon(HeavyWeapon usedWeapon);
    public abstract void VisitLightWeapon(LightWeapon usedWeapon);
    public abstract void VisitMagicalWeapon(MagicalWeapon usedWeapon);
    public abstract void VisitOtherUsable(IUsable usedWeapon);

    public abstract void Attack();
}
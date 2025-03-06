namespace Project_oob.Items;

public abstract class Weapon : Item, IUsable
{
    private int _damage;
    
    public abstract void Pickup();

    public abstract void Use();
}

public abstract class TwoHandedWeapon : Weapon
{
    
}


public abstract class OneHandedWeapon : Weapon
{
    
}
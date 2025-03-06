namespace Project_oob.Items;

public abstract class Money : Item, IUsable
{
    private int _value;

    public abstract void Pickup();

    public abstract void Use();
}

public class Coin : Money
{
    public override void Pickup()
    {
        throw new NotImplementedException();
    }

    public override void Use()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}

public class Gold : Money
{
    public override void Pickup()
    {
        throw new NotImplementedException();
    }

    public override void Use()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
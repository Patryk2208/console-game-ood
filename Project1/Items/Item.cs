using Project_oob.Map;

namespace Project_oob.Items;

public abstract class Item : MapElement
{
    public string Name { get; protected set; }
}

public interface IPickupable
{
    void Pickup();
}

public interface INonPickupable
{
    
}

public interface IUsable : IPickupable
{
    void Use();
}


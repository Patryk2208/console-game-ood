using Project_oob.Items;

namespace Project_oob.Beings;

public class Body
{
    public Dictionary<string, BodyPart> BodyParts { get; protected set; } = new();

    public void AddBodyPart(BodyPart bp)
    {
        BodyParts.Add(bp.Name, bp);
    }
}

public abstract class BodyPart
{
    public string Name { get; protected init; }
    public Item? usedItem { get; protected set; }

    public bool IsUsed => usedItem != null;
    public void PutOn(Item item)
    {
        usedItem = item;
    }

    public Item TakeOff()
    {
        var res = usedItem;
        usedItem = null;
        return res;
    }
}

public class Hand : BodyPart
{
    
}

public class LeftHand : Hand
{
    public LeftHand()
    {
        Name = "LeftHand";
    }
}

public class RightHand : Hand
{
    public RightHand()
    {
        Name = "RightHand";
    }
}
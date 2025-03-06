using Project_oob.Items;

namespace Project_oob.Beings;

public class Body
{
    protected Dictionary<string, BodyPart> BodyParts = new();

    public void AddBodyPart(BodyPart bp)
    {
        BodyParts.Add(bp.Name, bp);
    }
}

public abstract class BodyPart
{
    public readonly string Name;
    IUsable usedItem;

    public void PutOn(IUsable item)
    {
        usedItem = item;
    }
}

public class Hand : BodyPart
{
    
}

public class LeftHand : Hand
{
    public new readonly string Name;
    public LeftHand()
    {
        Name = "LeftHand";
    }
}

public class RightHand : Hand
{
    public new readonly string Name;
    public RightHand()
    {
        Name = "RightHand";
    }
}
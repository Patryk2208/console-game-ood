using RPG_ood.Items;

namespace RPG_ood.Beings;

public class Body
{
    public Dictionary<string, BodyPart> BodyParts { get; protected set; } = new();

    public void AddBodyPart(BodyPart bp)
    {
        BodyParts.Add(bp.Name, bp);
    }

    public int GetArmor()
    {
        var armor = 0;
        foreach (var part in BodyParts.Values)
        {
            armor += part.GetArmor();
        }
        return armor;
    }
}

public abstract class BodyPart
{
    public string Name { get; protected init; }
    public IUsable? usedItem { get; protected set; }

    public bool IsUsed => usedItem != null;
    public void PutOn(IUsable item)
    {
        usedItem = item;
    }
    public IPickupable TakeOff()
    {
        IPickupable res = usedItem!;
        usedItem = null;
        return res;
    }

    public int GetArmor()
    {
        if(!IsUsed) return 0;
        var usedItemName = usedItem!.Name;
        return usedItemName.Contains("Defensive") ? usedItem.Damage : 0;
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
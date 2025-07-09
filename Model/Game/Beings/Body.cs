using System.Text.Json.Serialization;
using Model.Game.Items;

namespace Model.Game.Beings;

[Serializable]
public class Body
{
    public Dictionary<string, BodyPart> BodyParts { get; protected set; }

    public Body()
    {
        BodyParts = new();
    }

    [JsonConstructor]
    public Body(Dictionary<string, BodyPart> bodyParts)
    {
        BodyParts = bodyParts;
    }

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

[JsonDerivedType(typeof(LeftHand), "LeftHand")]
[JsonDerivedType(typeof(RightHand), "RightHand")]
public abstract class BodyPart
{
    public string Name { get; protected init; }
    public IUsable? UsedItem { get; protected set; }
    [JsonIgnore]
    public bool IsUsed => UsedItem != null;

    public BodyPart() {}
    [JsonConstructor]
    public BodyPart(string name, IUsable? usedItem)
    {
        Name = name;
        UsedItem = usedItem;
    }
    public void PutOn(IUsable item)
    {
        UsedItem = item;
    }
    public IPickupable TakeOff()
    {
        IPickupable res = UsedItem!;
        UsedItem = null;
        return res;
    }

    public int GetArmor()
    {
        if(!IsUsed) return 0;
        var usedItemName = UsedItem!.Name;
        return usedItemName.Contains("Defensive") ? UsedItem.Damage : 0;
    }
}

public class Hand : BodyPart
{
    public Hand() {}
    [JsonConstructor]
    public Hand(string name, IUsable? usedItem) : base(name, usedItem) {}
}

public class LeftHand : Hand
{
    public LeftHand()
    {
        Name = "LeftHand";
    }
    [JsonConstructor]
    public LeftHand(string name, IUsable? usedItem) : base(name, usedItem) {}
}

public class RightHand : Hand
{
    public RightHand()
    {
        Name = "RightHand";
    }
    [JsonConstructor]
    public RightHand(string name, IUsable? usedItem) : base(name, usedItem) {}
}
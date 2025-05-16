using System.Text.Json.Serialization;
using RPG_ood.Model.Game.Items;
using RPG_ood.Model.Items;

namespace RPG_ood.Model.Beings;

[Serializable]
public class Equipment
{
    public List<IPickupable> Eq { get; private set; }
    public List<IValuable> Sack { get; private set; }
    public int GoldCount { get; set; }
    public int CoinCount { get; set; }
    public int EqPointer { get; set; }
    public int SackValue { get; private set; }

    public Equipment()
    {
        Eq = new();
        Sack = new();
        SackValue = 0;
    }

    [JsonConstructor]
    public Equipment(List<IPickupable> eq, List<IValuable> sack, int goldCount, int coinCount, int eqPointer,
        int sackValue)
    {
        Eq = eq;
        Sack = sack;
        GoldCount = goldCount;
        CoinCount = coinCount;
        EqPointer = eqPointer;
        SackValue = sackValue;
    }
    public void TryMovePointerLeft()
    {
        if (EqPointer > 0)
        {
            EqPointer--;
        }
    }
    
    public void TryMovePointerRight()
    {
        if (EqPointer < Eq.Count - 1)
        {
            EqPointer++;
        }
    }
    
    public void AddItemToEq(IPickupable item)
    {
        Eq.Add(item);
    }
    public void AddItemToSack(IValuable item)
    {
        Sack.Add(item);
        SackValue += item.Value;
    }
    public IItem RemoveItemFromEq()
    {
        var res = Eq[EqPointer];
        Eq.RemoveAt(EqPointer);
        TryMovePointerLeft();
        return res;
    }
}
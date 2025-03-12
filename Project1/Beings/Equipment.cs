using Project_oob.Items;

namespace Project_oob.Beings;

public class Equipment
{
    public List<Item> Eq { get; private set; } = new();
    public List<IValuable> Sack { get; private set; } = new();
    public int GoldCount { get; set; }
    public int CoinCount { get; set; }
    public int EqPointer { get; set; }
    public int SackValue { get; private set; } = 0;

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
    
    public void AddItemToEq(Item item)
    {
        Eq.Add(item);
    }
    public void AddItemToSack(IValuable item)
    {
        Sack.Add(item);
        SackValue += item.Value;
    }
    public Item RemoveItemFromEq()
    {
        var res = Eq[EqPointer];
        Eq.RemoveAt(EqPointer);
        TryMovePointerLeft();
        return res;
    }
}
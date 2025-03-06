using Project_oob.Items;

namespace Project_oob.Beings;

public class Equipment
{
    private List<Item> _items = new();
    

    public void AddItem(Item item)
    {
        _items.Add(item);
    }

    public Item RemoveItem(int index)
    {
        var Res = _items[index];
        _items.RemoveAt(index);
        return Res;
    }
}
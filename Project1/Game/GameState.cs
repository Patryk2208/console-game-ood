using Project_oob.Beings;
using Project_oob.Items;
using Project_oob.Map;
using System.Linq;
using Project_oob.Effects;

namespace Project_oob.Game;

public abstract class GameState
{
    public abstract List<Being> Beings { get; protected set; }
    public abstract List<Item> Items { get; protected set; }
    public abstract World World { get; protected set; }
    public abstract Map.Map CurrentMap { get; protected set; }
    public abstract Room CurrentRoom { get; protected set; }
    public int SamePosIndex { get; set; }
    public abstract IEnumerable<Item> GetItemsAtPos(Position pos);
}

public class SinglePlayerGameState : GameState
{
    public override List<Being> Beings { get; protected set; }
    public override List<Item> Items { get; protected set; }
    public override World World { get; protected set; }
    public override Map.Map CurrentMap { get; protected set; }
    public override Room CurrentRoom { get; protected set; }

    public SinglePlayerGameState(Player player, World world, int itemsCount)
    {
        World = world;
        CurrentMap = World.Maps.First().Value;
        CurrentRoom = CurrentMap.Rooms.First();
        Beings = [player];
        Items = [];
        SamePosIndex = 0;
        for (int i = 0; i < itemsCount; i++)
        {
            Items.Add(new Coin());
        }
        for (int i = 0; i < itemsCount; i++)
        {
            Items.Add(new Gold());
        }
        for (int i = 0; i < itemsCount; i++)
        {
            Items.Add(new OffensiveWeapon(new LuckyWeapon(new Sword())));
        }
        for (int i = 0; i < itemsCount; i++)
        {
            Items.Add(new LightWeapon(new Knife()));
        }
        for (int i = 0; i < itemsCount; i++)
        {
            Items.Add(new HeavyWeapon(new BigSword()));
        }
        for (int i = 0; i < itemsCount; i++)
        {
            Items.Add(new DefensiveWeapon(new Shield()));
        }
        for (int i = 0; i < itemsCount; i++)
        {
            Items.Add(new MagicMineral(new Sand()));
        }
        for (int i = 0; i < itemsCount; i++)
        {
            Items.Add(new Water());
        }
        for (int i = 0; i < itemsCount; i++)
        {
            Items.Add(new Wood());
        }
        PlaceItems(CurrentRoom);
    }

    public void PlaceItems(Room room)
    {
        List<(int, int)> availableSlots = new List<(int, int)>();
        for (int i = 0; i < room.Height; i++)
        {
            for (int j = 0; j < room.Width; j++)
            {
                if (room.Elements[i, j].OnStandable())
                {
                    availableSlots.Add((i, j));
                }
            }
        }
        var rand = new Random();
        foreach (Item t in Items)
        {
            var nextInd = rand.Next(0, availableSlots.Count);
            var pos = availableSlots[nextInd];
            t.Pos = new Position(pos.Item1, pos.Item2);
        }
        
    }

    public override IEnumerable<Item> GetItemsAtPos(Position pos)
    {
        var res = Items.Where(i => i.Pos.Equals(pos)).ToArray();
        var count = res.Length;
        if (count > 0 && count - 1 < SamePosIndex)
        {
            SamePosIndex = count - 1;
        }
        return res;
    }
    
}
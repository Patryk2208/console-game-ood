using Model.Communication.Snapshots;
using Model.Game;
using Model.Game.Beings;
using Model.Game.Items;
using Model.Game.Map;

namespace Model.RelativeGameState;

public class RelativeRoomState
{
    public string Name { get; protected set; }
    public int Width { get; }
    public int Height { get; }
    //public Instruction RoomInstruction { get; set; }
    public MapElement[,] Elements { get; protected set; }
    public List<IBeing> Beings { get; protected set; }
    public List<IItem> Items { get; protected set; }
    public List<PlayerSnapshot> Players { get; protected set; }

    public RelativeRoomState(RoomSnapshot roomSnapshot)
    {
        Name = roomSnapshot.Name;
        Width = roomSnapshot.Width;
        Height = roomSnapshot.Height;
        Elements = roomSnapshot.Elements;
        Beings = roomSnapshot.Beings;
        Items = roomSnapshot.Items;
        Players = roomSnapshot.Players;
    }
    public IEnumerable<IItem> GetItemsAtPos(Position pos)
    {
        var res = Items.Where(i => i.Pos.IsSet() && i.Pos.Equals(pos)).ToArray();
        var count = res.Length;
        return res;
    }

    public IEnumerable<IBeing> GetBeingsNearby(Position pos, float radius)
    {
        return Beings.Where(b => b.Pos.IsSet() && Math.Pow(b.Pos.X - pos.X, 2) + Math.Pow(b.Pos.Y - pos.Y, 2) < radius);
    }
}
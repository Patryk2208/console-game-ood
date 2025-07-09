using Model.Game.Beings;
using Model.Game.GameState;
using Model.Game.Items;

namespace Model.Game.Map;


public interface IMappable
{
    public Position Pos { get; set; }
}

public class World
{
    public Dictionary<string, Map> Maps = new();
}

public class Map
{
    public string Name { get; protected set; }
    public int RoomCount { get; protected set; }
    public Room[] Rooms { get; protected set; }

    public Map(string name, int roomCount)
    {
        Name = name;
        RoomCount = roomCount;
        Rooms = new Room[RoomCount];
    }
}

public class Room(string name, int width, int height)
{
    public string Name { get; protected set; } = name;
    public int Width { get; } = width;
    public int Height { get; } = height;
    public Instruction RoomInstruction { get; set; }
    public MapElement[,] Elements { get; protected set; } = new MapElement[height, width];
    public List<IBeing> Beings { get; protected set; } = new();
    public List<IItem> Items { get; protected set; } = new();
    public List<Player> Players { get; protected set; } = new();
    public IEnumerable<IItem> GetItemsAtPos(Position pos)
    {
        var res = Items.Where(i => i.Pos.IsSet() && i.Pos.Equals(pos)).ToArray();
        var count = res.Length;
        return res;
    }

    public IEnumerable<IBeing> GetBeingsNearby(Position pos, float radius)
    {
        return Beings.Where(b => b.Pos.IsSet() && 
                                 Math.Pow(b.Pos.X - pos.X, 2) + Math.Pow(b.Pos.Y - pos.Y, 2) < radius);
    }

    public IEnumerable<Player> GetPlayersNearby(Position pos, float radius)
    {
        return Players.Where(p => p.Pos.IsSet() && !p.Pos.Equals(pos) && 
                                  Math.Pow(p.Pos.X - pos.X, 2) + Math.Pow(p.Pos.Y - pos.Y, 2) < radius);
    }

        /*for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Elements[i, j] = new BlankMapElement();
            }
        }
        var rand = new Random();
        for (int x = 0; x < 2 * (width + height); x++)
        {
            var wallPos = new Position(rand.Next(0, height), rand.Next(0, width));
            Elements[wallPos.X, wallPos.Y] = new Wall(32 + rand.Next(0, 2));
        }*/
}
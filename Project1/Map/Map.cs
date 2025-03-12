namespace Project_oob.Map;


public interface IMappable
{
    public Position Pos { get; set; }
    public string ToString();
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

public class Room
{
    public string Name { get; protected set; }
    public int Width { get; }
    public int Height { get; }
    public MapElement[,] Elements { get; protected set; }

    public Room(string name, int width, int height)
    {
        Name = name;
        Width = width;
        Height = height;
        Elements = new MapElement[height, width];
        for (int i = 0; i < height; i++)
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
        }
    }
}
namespace Project_oob.Map;


public class World
{
    protected Dictionary<string, Map> Maps = new();
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
        Elements = new MapElement[width, height];
    }
}
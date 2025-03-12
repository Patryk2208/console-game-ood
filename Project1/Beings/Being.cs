using Project_oob.Map;

namespace Project_oob.Beings;

public abstract class Being : IMappable
{
    public string Name { get; protected init; }
    public Position Pos { get; set; }
    public int Color { get; protected init; }
    public abstract override string ToString();
}


public abstract class Npc : Being
{
    //later
}
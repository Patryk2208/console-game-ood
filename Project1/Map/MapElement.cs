namespace Project_oob.Map;

public interface IMappable
{
    public Position? GetPosition();
}


public abstract class MapElement : IMappable
{
    protected Position? Pos;

    public Position? GetPosition() => Pos;
    
    public void SetPosition(Position? pos)
    {
        Pos = pos;
    }

    public abstract override string ToString();
}

public class BlankMapElement : MapElement
{
    public override string ToString()
    {
        return " ";
    }
}

public class Wall : MapElement
{
    public override string ToString()
    {
        return "\u2588";
    }
}
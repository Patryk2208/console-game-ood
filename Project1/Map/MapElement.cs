namespace Project_oob.Map;

public abstract class MapElement
{
    public abstract bool OnStandable(); 
    public int color { get; set; }
    public abstract override string ToString();
}

public class BlankMapElement : MapElement
{
    public override bool OnStandable() => true;
    public override string ToString()
    {
        return " ";
    }
}

public class Wall : MapElement
{
    public Wall(int color) => this.color = color;
    public override bool OnStandable() => false;
    public override string ToString()
    {
        return "\u2588";
    }
}
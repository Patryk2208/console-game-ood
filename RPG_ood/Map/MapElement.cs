namespace RPG_ood.Map;

public abstract class MapElement
{
    public abstract bool OnStandable { get; set; }
    public int color { get; set; }
    public abstract override string ToString();
}

public class BlankMapElement : MapElement
{
    public override bool OnStandable { get; set; } = true;
    public override string ToString()
    {
        return " ";
    }
}

public class Wall : MapElement
{
    public Wall(int color) => this.color = color;
    public override bool OnStandable { get; set; } = false;

    public override string ToString()
    {
        return "\u2588";
    }
}
namespace RPG_ood.Map;

public abstract class MapElement
{
    public abstract bool OnStandable { get; set; }
    public AnsiConsoleColor Color { get; init; }
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
    public Wall(AnsiConsoleColor color) => this.Color = color;
    public override bool OnStandable { get; set; } = false;

    public override string ToString()
    {
        return "\u2588";
    }
}
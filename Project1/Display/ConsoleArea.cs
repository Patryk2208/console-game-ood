namespace Project_oob.Display;

public abstract class ConsoleArea
{
    public abstract Position StartPosition { get; protected set; }
    public abstract int Width { get; }
    public abstract int Height { get; }
    
    public ConsolePixel BlankPixel { get; protected init; } = new ConsolePixel();
}

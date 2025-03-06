namespace Project_oob.Display;

public interface IDisplay
{
    public void Display();
}


public class ConsoleDisplay : IDisplay
{
    public Position StartPosition { get; protected set; }
    public int Width { get; }
    public int Height { get; }
    public ConsoleDisplayGame Game { get; protected set; }
    public ConsoleDisplayMap Map { get; protected set; }
    public ConsoleDisplayStatus Status { get; protected set; }

    public char[,] GameBoard { get; set; }

    public ConsoleDisplay(int w, int h)
    {
        StartPosition = new Position(0, 0);
        Width = w;
        Height = h;
        GameBoard = new char[w, h];
        Console.Write(GameBoard);
        Console.Write(@"\033[?1049h");
        Console.Write($@"\033[{StartPosition.X};{StartPosition.Y}");
        //Console.SetCursorPosition(StartPosition.X, StartPosition.Y);
        Game = new ConsoleDisplayGame(100, 50);
        Game.Display();
        Map = new ConsoleDisplayMap();
        Status = new ConsoleDisplayStatus();
    }
    
    public void Display()
    {
        throw new NotImplementedException();
    }
}
using System.Text;
using Project_oob.Beings;
using Project_oob.Game;
using Project_oob.Map;

namespace Project_oob.Display;

public interface IDisplay
{
    public void Display();
}

public static class ConsoleWriter
{
    public static void InsertText(ref ConsolePixel[,] consolePixels, Position pos, string text, int color = 37)
    {
        for (int i = 0; i < text.Length; i++)
        {
            try
            {
                consolePixels[pos.X, pos.Y + i] = new ConsolePixel(color, text[i]);
            }
            catch (Exception)
            {
                break;
            }
        }
    }
}
public class ConsolePixel(int cc = 37, char c = (char)32)
{
    public int Color { get; set; } = cc;
    public char Symbol { get; set; } = c;

    public override string ToString()
    {
        return $"\x1B[{Color}m{Symbol}";
    }
}
public class ConsoleDisplay : ConsoleArea, IDisplay
{
    public override Position StartPosition { get; protected set; }
    public override int Width { get; }
    public override int Height { get; }
    private ConsolePixel[,] _gameBoard;
    public ConsolePixel[,] GameBoard
    {
        get => _gameBoard; 
        protected set => _gameBoard = value;
    }
    public StringBuilder GameBoardString { get; protected set; }

    //beta
    public ConsoleMapArea MapArea { get; protected set; }
    public ConsoleStatusBarArea StatusBarArea { get; protected set; }

    protected CancellationTokenSource _cts;
    protected Mutex _mutex;

    public ConsoleDisplay(int width, int height, SinglePlayerGameState state, Room room, 
        CancellationTokenSource cts, Mutex mutex)
    {
        StartPosition = new Position(0, 0);
        _cts = cts;
        _mutex = mutex;
        Width = width;
        Height = height;
        _gameBoard = new ConsolePixel[height, width];
        GameBoardString = new StringBuilder();
        MapArea = new ConsoleMapArea(room, ref _gameBoard, new Position(3, 1));
        StatusBarArea = new ConsoleStatusBarArea(state, ref _gameBoard, Width - MapArea.Width - 3, MapArea.Height, 
            new Position(MapArea.StartPosition.X, MapArea.StartPosition.Y + MapArea.Width + 1));
    }
    
    public async Task Run()
    {
        Console.Write("\x1B[?1049h");
        Console.Write("\x1B[?25l");
        Console.Write("\x1B[2J");
        Console.Write("\x1B[H");
        BuildFrame();
        await Task.Run(
            () =>
            {
                while (_cts.Token.IsCancellationRequested == false)
                {
                    Console.SetCursorPosition(StartPosition.X, StartPosition.Y);
                    Thread.Sleep(17);
                    _mutex.WaitOne();
                    Display();
                    _mutex.ReleaseMutex();
                }
            }, _cts.Token);
        Console.Write("\x1B[?25h");
        Console.Write("\x1B[?1049l");
    }
    
    public void Display()
    {
        GameBoardString.Clear();
        StatusBarArea.DrawStatusBar();
        MapArea.DrawGameBoard();

        foreach (var item in StatusBarArea.CurrentState.Items)
        {
            if(item.Pos.IsSet())
            {
                GameBoard[MapArea.StartPosition.X + item.Pos.X, MapArea.StartPosition.Y + item.Pos.Y] =
                    new ConsolePixel(item.Color, item.ToString()[0]);
            }
        }
        
        foreach (var player in StatusBarArea.CurrentState.Beings)
        {
            GameBoard[MapArea.StartPosition.X + player.Pos.X, MapArea.StartPosition.Y + player.Pos.Y] =
                new ConsolePixel(player.Color, player.ToString()[0]);
        }
        
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                GameBoard[i, j] ??= BlankPixel;
                GameBoardString.Append(GameBoard[i, j].ToString());
            }
            GameBoardString.Append('\n');
        }
        
        Console.Write(GameBoardString.ToString());
    }

    private void BuildFrame()
    {
        var horizontalFrame = new ConsolePixel(37, '_');
        var verticalFrame = new ConsolePixel(37, '|');
        for (int i = 0; i < Width; i++)
        {
            GameBoard[0, i] = horizontalFrame;
            GameBoard[2, i] = horizontalFrame;
            GameBoard[MapArea.StartPosition.X + MapArea.Height, i] = horizontalFrame;
            GameBoard[Height - 1, i] = horizontalFrame;
        }

        ConsoleWriter.InsertText(ref _gameBoard, new Position(1, 3), "Game: 1");
        ConsoleWriter.InsertText(ref _gameBoard, new Position(1, 15), $"Room: {MapArea.CurrentRoom.Name}", 32);
        for (int i = 1; i < Height - 1; i++)
        {
            GameBoard[i, 0] = verticalFrame;
            GameBoard[i, MapArea.StartPosition.Y + MapArea.Width] = verticalFrame;
            GameBoard[i, StatusBarArea.StartPosition.Y - 1] = verticalFrame;
            GameBoard[i, Width - 1] = verticalFrame;
        }
    }
}
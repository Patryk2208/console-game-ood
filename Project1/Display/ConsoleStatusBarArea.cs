using Project_oob.Beings;
using Project_oob.Game;
using Project_oob.Items;

namespace Project_oob.Display;

public class ConsoleStatusBarArea : ConsoleArea
{
    public override Position StartPosition { get; protected set; }
    public override int Height { get; }
    public override int Width { get; }
    private ConsolePixel[,] _gameBoard;
    public ConsolePixel[,] GameBoard
    {
        get => _gameBoard; 
        protected set => _gameBoard = value;
    }
    public GameState CurrentState { get; }
    
    public ConsoleStatusBarArea(GameState state, ref ConsolePixel[,] gameBoard, int width, int height, Position startPosition)
    {
        CurrentState = state;
        Width = width;
        Height = height;
        StartPosition = startPosition;
        _gameBoard = gameBoard;
    }

    public void DrawStatusBar()
    {
        for (int i = StartPosition.X; i < StartPosition.X + Height - 1; i++)
        {
            for (int j = StartPosition.Y; j < StartPosition.Y + Width - 1; j++)
            {
                GameBoard[i, j] = BlankPixel;
            }
        }

        Player a = (Player)CurrentState.Beings[0]; //todo: eliminate explicitness
        var coursor = new Position(StartPosition.X, StartPosition.Y);
        ConsoleWriter.InsertText(ref _gameBoard, coursor, a.Name);
        foreach (var attr in a.GetAttributes())
        {
            coursor = coursor with { X = coursor.X + 1 };
            ConsoleWriter.InsertText(ref _gameBoard, coursor, $"{attr.Key}: {attr.Value}");
        }
        foreach (var part in a.Bd.BodyParts)
        {
            coursor = coursor with { X = coursor.X + 1 };
            string val;
            if (part.Value.usedItem == null) val = "";
            else val = part.Value.usedItem.PrintName();
            ConsoleWriter.InsertText(ref _gameBoard, coursor, $"{part.Key}: {val}");
        }
        coursor = coursor with { X = coursor.X + 1 };
        ConsoleWriter.InsertText(ref _gameBoard, coursor, $"Sack value: {a.Eq.SackValue}");
        coursor = coursor with { X = coursor.X + 1 };
        ConsoleWriter.InsertText(ref _gameBoard, coursor, "EQ:");
        int counter = 0;
        foreach (var it in a.Eq.Eq)
        {
            coursor = coursor with { X = coursor.X + 1 };
            ConsoleWriter.InsertText(ref _gameBoard, coursor, it.PrintName(), counter == a.Eq.EqPointer ? 33 : 37);
            ++counter;
        }
        
        var onSamePos = CurrentState.GetItemsAtPos(a.Pos);
        
        coursor = coursor with { X = coursor.X + 2 };
        counter = 0;
        foreach (var item in onSamePos)
        {
            coursor = coursor with { X = coursor.X + 1 };
            ConsoleWriter.InsertText(ref _gameBoard, coursor, item.PrintName(), counter == CurrentState.SamePosIndex ? 33 : 37);
            ++counter;
        }
    }
    
}
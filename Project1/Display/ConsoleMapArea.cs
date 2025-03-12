using Project_oob.Map;

namespace Project_oob.Display;


public class ConsoleMapArea : ConsoleArea
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
    public Room CurrentRoom { get; }  //too specific

    public ConsoleMapArea(Room room, ref ConsolePixel[,] gameBoard, Position startPosition)
    {
        CurrentRoom = room;
        Width = room.Width;
        Height = room.Height;
        StartPosition = startPosition;
        _gameBoard = gameBoard;
    }

    public void DrawGameBoard()
    {
        var rand = new Random();
        for (int i = StartPosition.X, iEl = 0; i < StartPosition.X + Height; i++, iEl++)
        {
            for (int j = StartPosition.Y, jEl = 0; j < StartPosition.Y + Width; j++, jEl++)
            {
                GameBoard[i, j] = new ConsolePixel(CurrentRoom.Elements[iEl, jEl].color, 
                    CurrentRoom.Elements[iEl, jEl].ToString()[0]);
            }
        }
    }
}
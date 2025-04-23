using System.Reflection;
using System.Text;
using RPG_ood.Beings;
using RPG_ood.Game;
using RPG_ood.Map;
using RPG_ood.Input;

namespace RPG_ood.Display;

public static class ConsoleWriter
{
    public static void InsertText(ref ConsolePixel[,] consolePixels, Position pos, string text, AnsiConsoleColor color = AnsiConsoleColor.White)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (pos.X >= 0 & pos.X < consolePixels.GetLength(0) && pos.Y >= 0 & pos.Y < consolePixels.GetLength(1))
            {
                consolePixels[pos.X, pos.Y + i] = new ConsolePixel(color, text[i]);
            }
            else
            {
                break;
            }
        }
    }
}

public class ConsolePixel(AnsiConsoleColor cc = AnsiConsoleColor.White, char c = (char)32)
{
    private AnsiConsoleColor Color { get; set; } = cc;
    private char Symbol { get; set; } = c;

    public override string ToString()
    {
        return $"\x1B[{(int)Color}m{Symbol}";
    }
}


public class Display
{
    private static Display? Instance { get; set; }
    private int Width { get; } = 110;
    private int Height { get; } = 26;
    private ConsolePixel[,] _gameBoard;
    public ConsolePixel[,] GameBoard
    {
        get => _gameBoard;
        protected set => _gameBoard = value;
    }
    private ConsolePixel _blankPixel = new ConsolePixel();
    /*public Position DisplayPosition { get; set; }
    public Position StatusPostion { get; set; }
    public Position MapPostion { get; set; }*/
    private int _roomSPX;
    private StringBuilder GameBoardString { get; set; }

    private Display()
    {
        _gameBoard = new ConsolePixel[Height, Width];
        GameBoardString = new();
    }
    public static Display GetInstance()
    {
        if (Instance == null)
        {
            Instance = new();
        }
        return Instance!;
    }

    public void PrepareGame()
    {
        Console.Write("\x1B[?1049h");
        Console.Write("\x1B[?25l");
        Console.Write("\x1B[2J");
        Console.Write("\x1B[H");
    }

    public void DisplayGame()
    {
        GameBoardString.Clear();
        Console.SetCursorPosition(0, 0);
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                GameBoard[i, j] ??= _blankPixel;
                GameBoardString.Append(GameBoard[i, j].ToString());
            }
            GameBoardString.Append('\n');
        }
        Console.Write(GameBoardString.ToString());
    }

    public void CleanupGame()
    {
        Console.Write("\x1B[?25h");
        Console.Write("\x1B[?1049l");
    }

    //general display section - refreshable between room changes
    public void DisplayFrame(GameState state)
    {
        var horizontalFrame = new ConsolePixel(AnsiConsoleColor.White, '_');
        var verticalFrame = new ConsolePixel(AnsiConsoleColor.White, '|');
        _roomSPX = Height - state.CurrentRoom.Height - 2;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                GameBoard[i, j] = _blankPixel;
            }
        }
        for (int i = 0; i < Width; i++)
        {
            GameBoard[0, i] = horizontalFrame;
            if (i < state.CurrentRoom.Width)
                GameBoard[_roomSPX, i] = horizontalFrame;
            GameBoard[_roomSPX + 1 + state.CurrentRoom.Height, i] = horizontalFrame;
        }

        for (int i = 1; i < Height - 1; i++)
        {
            GameBoard[i, 0] = verticalFrame;
            GameBoard[i, state.CurrentRoom.Width + 1] = verticalFrame;
            GameBoard[i, Width - 1] = verticalFrame;
        }
    }

    public void DisplayMapInfo(GameState state)
    {
        ConsoleWriter.InsertText(ref _gameBoard, new Position(_roomSPX / 2, 2), $"{state.CurrentMap.Name}");
        ConsoleWriter.InsertText(ref _gameBoard, new Position(_roomSPX / 2, state.CurrentRoom.Width / 2 + 1),
            $"{state.CurrentRoom.Name}");
    }

    //universal status display section
    public void RefreshPlayerInfo(GameState state)
    {
        var coursor = new Position(_roomSPX / 2, state.CurrentRoom.Width + 2);
        ConsoleWriter.InsertText(ref _gameBoard, coursor, state.Player.Name);
        coursor = coursor with { X = coursor.X + 1 };
        foreach (var attr in state.Player.GetAttributes())
        {
            coursor = coursor with { X = coursor.X + 1 };
            ConsoleWriter.InsertText(ref _gameBoard, coursor, $"{attr.Key}: {attr.Value}");
        }

        coursor = coursor with { X = coursor.X + 1 };
        ConsoleWriter.InsertText(ref _gameBoard, coursor, $"Sack value: {state.Player.Eq.SackValue}");
        coursor = coursor with { X = coursor.X + 1 };
        ConsoleWriter.InsertText(ref _gameBoard, coursor, $"Coins: {state.Player.Eq.CoinCount}");
        coursor = coursor with { X = coursor.X + 1 };
        ConsoleWriter.InsertText(ref _gameBoard, coursor, $"Gold: {state.Player.Eq.GoldCount}");
        
        coursor = coursor with { X = _roomSPX / 2, Y = state.CurrentRoom.Width + 20 };
        foreach (var part in state.Player.Bd.BodyParts)
        {
            string val;
            if (part.Value.usedItem == null) val = "";
            else val = part.Value.usedItem.PrintName();
            ConsoleWriter.InsertText(ref _gameBoard, coursor, $"{part.Key}: {val}");
            coursor = coursor with { X = coursor.X + 1 };
        }
        ConsoleWriter.InsertText(ref _gameBoard, coursor, "EQ:");
        int counter = 0;
        foreach (var it in state.Player.Eq.Eq)
        {
            coursor = coursor with { X = coursor.X + 1 };
            ConsoleWriter.InsertText(ref _gameBoard, coursor, it.PrintName(),
                counter == state.Player.Eq.EqPointer ? AnsiConsoleColor.Yellow : AnsiConsoleColor.White);
            ++counter;
        }
        
        
        //todo better elixir effects display
        coursor = coursor with { X = coursor.X = _roomSPX / 2, Y = state.CurrentRoom.Width + 50 };
        ConsoleWriter.InsertText(ref _gameBoard, coursor, $"Applied Effects:");
        foreach (var effectName in state.Player.MomentChangedEvent.Names)
        {
            coursor = coursor with { X = coursor.X + 1 };
            ConsoleWriter.InsertText(ref _gameBoard, coursor, $"{effectName}");
        }
    }

    //map display section
    public void RefreshRoom(Room room)
    {
        for (int i = _roomSPX + 1, iEl = 0; i < _roomSPX + 1 + room.Height; i++, iEl++)
        {
            for (int j = 1, jEl = 0; j < 1 + room.Width; j++, jEl++)
            {
                GameBoard[i, j] = new ConsolePixel(room.Elements[iEl, jEl].Color,
                    room.Elements[iEl, jEl].ToString()[0]);
            }
        }
    }

    public void RefreshItems(Room room)
    {
        foreach (var item in room.Items)
        {
            if (item.Pos.IsSet())
            {
                GameBoard[_roomSPX + 1 + item.Pos.X, 1 + item.Pos.Y] =
                    new ConsolePixel(item.Color, item.ToString()[0]);
            }
        }
    }

    public void RefreshBeings(Room room)
    {
        foreach (var being in room.Beings)
        {
            if (being.Pos.IsSet())
            {
                GameBoard[_roomSPX + 1 + being.Pos.X, 1 + being.Pos.Y] =
                    new ConsolePixel(being.Color, being.ToString()[0]);
            }
        }
    }

    public bool RefreshPlayers(Player p)
    {
        if (p.Pos.IsSet())
        {
            GameBoard[_roomSPX + 1 + p.Pos.X, 1 + p.Pos.Y] =
                new ConsolePixel(p.Color, p.ToString()[0]);
            return true;
        }
        var coursor = new Position(Height / 2, Width / 2);
        ConsoleWriter.InsertText(ref _gameBoard, coursor, "GAME OVER", AnsiConsoleColor.Red);
        return false;
    }
    //specific status display section

    public void RefreshItemsOnPosition(GameState state)
    {
        var onSamePos = state.CurrentRoom.GetItemsAtPos(state.Player.Pos);

        var coursor = new Position(_roomSPX + state.CurrentRoom.Height / 2 + 2, state.CurrentRoom.Width + 2);
        var counter = 0;
        foreach (var item in onSamePos)
        {
            coursor = coursor with { X = coursor.X + 1 };
            ConsoleWriter.InsertText(ref _gameBoard, coursor, item.PrintName(),
                counter == state.CurrentRoom.PickUpCursor ? AnsiConsoleColor.Yellow : AnsiConsoleColor.White);
            ++counter;
        }
    }

    public void RefreshEnemiesNearby(GameState state)
    {
        var beingsNearby = state.CurrentRoom.GetBeingsNearby(state.Player.Pos, 17);

        var coursor = new Position(_roomSPX + state.CurrentRoom.Height / 2 + 2, state.CurrentRoom.Width + 30);
        foreach (var being in beingsNearby)
        {
            coursor = coursor with { X = coursor.X + 1 };
            ConsoleWriter.InsertText(ref _gameBoard, coursor, being.Name);
        }
    }

    //instructions and logs display section - refreshable between room changes
    public void DisplayInstructions(Instruction instructions)
    {
        Console.SetCursorPosition(0, Height + 1);
        foreach (var instruction in instructions.Instructions)
        {
            Console.WriteLine(instruction);
        }
    }

    public void DisplayLog(Logs logs)
    {
        var coursorXpos = 5;
        Console.SetCursorPosition(Width + 2, coursorXpos);
        var blank = new StringBuilder();
        foreach (var log in logs.LogMessgaes)
        {
            Console.Write(log);
            Console.SetCursorPosition(Width + 2, ++coursorXpos);
        }
    }
}
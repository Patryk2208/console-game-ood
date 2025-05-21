using System.Text;
using RPG_ood.Map;
using RPG_ood.Model;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game;
using RPG_ood.Model.Game.GameState;
using RPG_ood.Model.RelativeGameState;

namespace RPG_ood.View.Display;

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

public class ConsolePixel(AnsiConsoleColor cc = AnsiConsoleColor.White, char c = (char)32, AnsiConsoleColor? bgColor = null)
{
    private AnsiConsoleColor Color { get; init; } = cc;
    private AnsiConsoleColor? BgColor { get; init; } = bgColor;
    private char Symbol { get; set; } = c;
    
    private const string _reset = "\x1B[0m";
    public override string ToString()
    {
        if (BgColor.HasValue)
        {
            return $"\x1B[{(int)BgColor}m\x1B[{(int)Color}m{Symbol}{_reset}";
        }
        return $"\x1B[{(int)Color}m{Symbol}{_reset}";
    }
}


public class Display
{
    private static Display? Instance { get; set; }
    private int Width { get; } = 110; //cool 190
    private int Height { get; } = 26; //cool 36
    private ConsolePixel[,] _gameBoard;
    public ConsolePixel[,] GameBoard
    {
        get => _gameBoard;
        protected set => _gameBoard = value;
    }
    private ConsolePixel _blankPixel = new ConsolePixel();
    private int _roomSPX;
    private StringBuilder GameBoardString { get; set; }

    private ConsoleViewGenerator ViewGenerator { get; init; } = new();

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
    public void DisplayFrame(RelativeGameState state)
    {
        var horizontalFrame = new ConsolePixel(AnsiConsoleColor.White, '_');
        var verticalFrame = new ConsolePixel(AnsiConsoleColor.White, '|');
        _roomSPX = Height - state.CurrentRelativeRoom.Height - 2;
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
            if (i < state.CurrentRelativeRoom.Width)
                GameBoard[_roomSPX, i] = horizontalFrame;
            GameBoard[_roomSPX + 1 + state.CurrentRelativeRoom.Height, i] = horizontalFrame;
        }

        for (int i = 1; i < Height - 1; i++)
        {
            GameBoard[i, 0] = verticalFrame;
            GameBoard[i, state.CurrentRelativeRoom.Width + 1] = verticalFrame;
            GameBoard[i, Width - 1] = verticalFrame;
        }
    }

    public void DisplayMapInfo(RelativeGameState state)
    {
        /*ConsoleWriter.InsertText(ref _gameBoard, new Position(_roomSPX / 2, 2), $"{state.CurrentMap.Name}");
        ConsoleWriter.InsertText(ref _gameBoard, new Position(_roomSPX / 2, state.CurrentRoom.Width / 2 + 1),
            $"{state.CurrentRoom.Name}");*/
        ConsoleWriter.InsertText(ref _gameBoard, new Position(_roomSPX / 2, 2), $"{state.CurrentRelativeRoom.Name}");
    }

    //universal status display section
    public void RefreshPlayerInfo(RelativeGameState state)
    {
        var coursor = new Position(_roomSPX / 2, state.CurrentRelativeRoom.Width + 2);
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
        
        coursor = coursor with { X = _roomSPX / 2, Y = state.CurrentRelativeRoom.Width + 20 };
        foreach (var part in state.Player.Bd.BodyParts)
        {
            string val;
            if (part.Value.UsedItem == null) val = "";
            else val = part.Value.UsedItem.PrintName();
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
        coursor = coursor with { X = coursor.X = _roomSPX / 2, Y = state.CurrentRelativeRoom.Width + 50 };
        ConsoleWriter.InsertText(ref _gameBoard, coursor, $"Applied Effects:");
        //todo elixir effects fixable
        foreach (var effectName in state.AppliedEffects)
        {
            coursor = coursor with { X = coursor.X + 1 };
            ConsoleWriter.InsertText(ref _gameBoard, coursor, $"{effectName}");
        }
    }

    //map display section
    public void RefreshRoom(RelativeRoomState relativeRoom)
    {
        for (int i = _roomSPX + 1, iEl = 0; i < _roomSPX + 1 + relativeRoom.Height; i++, iEl++)
        {
            for (int j = 1, jEl = 0; j < 1 + relativeRoom.Width; j++, jEl++)
            {
                relativeRoom.Elements[iEl, jEl].AcceptView(ViewGenerator);
                GameBoard[i, j] = ViewGenerator.NewlyCreated;
            }
        }
    }

    public void RefreshItems(RelativeRoomState relativeRoom)
    {
        foreach (var item in relativeRoom.Items)
        {
            if (item.Pos.IsSet())
            {
                item.AcceptView(ViewGenerator);
                GameBoard[_roomSPX + 1 + item.Pos.X, 1 + item.Pos.Y] = ViewGenerator.NewlyCreated;
            }
        }
    }

    public void RefreshBeings(RelativeRoomState relativeRoom)
    {
        foreach (var being in relativeRoom.Beings)
        {
            if (being.Pos.IsSet())
            {
                being.AcceptView(ViewGenerator);
                GameBoard[_roomSPX + 1 + being.Pos.X, 1 + being.Pos.Y] = ViewGenerator.NewlyCreated;
            }
        }
    }

    public void RefreshPlayers(RelativeGameState state)
    {
        foreach (var p in state.CurrentRelativeRoom.Players)
        {
            if (p.Pos.IsSet())
            {
                p.AcceptView(ViewGenerator);
                GameBoard[_roomSPX + 1 + p.Pos.X, 1 + p.Pos.Y] = ViewGenerator.NewlyCreated;
            }
        }

        if (state.Player.IsDead)
        {

            var coursor = new Position(Height / 2, Width / 2);
            ConsoleWriter.InsertText(ref _gameBoard, coursor, "GAME OVER", AnsiConsoleColor.Red);
        }
    }
    //specific status display section

    public void RefreshItemsOnPosition(RelativeGameState state)
    {
        var onSamePos = state.Player.GetItemsAtPosFromRoom(state.CurrentRelativeRoom);

        var coursor = new Position(_roomSPX + state.CurrentRelativeRoom.Height / 2 + 2, state.CurrentRelativeRoom.Width + 2);
        var counter = 0;
        foreach (var item in onSamePos)
        {
            coursor = coursor with { X = coursor.X + 1 };
            ConsoleWriter.InsertText(ref _gameBoard, coursor, item.PrintName(),
                counter == state.Player.PickUpCursor ? AnsiConsoleColor.Yellow : AnsiConsoleColor.White);
            ++counter;
        }
    }

    public void RefreshEnemiesNearby(RelativeGameState state)
    {
        var beingsNearby = state.CurrentRelativeRoom.GetBeingsNearby(state.Player.Pos, 17);

        var coursor = new Position(_roomSPX + state.CurrentRelativeRoom.Height / 2 + 2, state.CurrentRelativeRoom.Width + 30);
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

    public void DisplayLog(RelativeLogs logs)
    {
        var cursorYpos = Width;
        var cursorXpos = Height + 3;
        Console.SetCursorPosition(cursorYpos, cursorXpos);
        foreach (var log in logs.LogMessages)
        {
            Console.Write(log);
            Console.SetCursorPosition(cursorYpos, ++cursorXpos);
        }
    }
}
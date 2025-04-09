using System.ComponentModel.Design;

namespace RPG_ood;

public struct Position(int x, int y) : IEquatable<Position>
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public bool IsSet() => X >= 0 && Y >= 0;
    public bool Equals(Position other) => X == other.X && Y == other.Y;
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
public enum AnsiConsoleColor
{
    // Standard foreground colors
    Black = 30,
    Red = 31,
    Green = 32,
    Yellow = 33,
    Blue = 34,
    Magenta = 35,
    Cyan = 36,
    White = 37,
    
    // Bright foreground colors
    BrightBlack = 90,
    BrightRed = 91,
    BrightGreen = 92,
    BrightYellow = 93,
    BrightBlue = 94,
    BrightMagenta = 95,
    BrightCyan = 96,
    BrightWhite = 97,
    
    // Standard background colors
    BgBlack = 40,
    BgRed = 41,
    BgGreen = 42,
    BgYellow = 43,
    BgBlue = 44,
    BgMagenta = 45,
    BgCyan = 46,
    BgWhite = 47,
    
    // Bright background colors
    BgBrightBlack = 100,
    BgBrightRed = 101,
    BgBrightGreen = 102,
    BgBrightYellow = 103,
    BgBrightBlue = 104,
    BgBrightMagenta = 105,
    BgBrightCyan = 106,
    BgBrightWhite = 107
}

/*public static class AnsiColor
{
    // 8-bit (256-color) mode
    public static string ForegroundColor(byte color) => $"\u001b[38;5;{color}m";
    public static string BackgroundColor(byte color) => $"\u001b[48;5;{color}m";
    
    // 24-bit (true color) mode
    public static string RgbForeground(byte r, byte g, byte b) => $"\u001b[38;2;{r};{g};{b}m";
    public static string RgbBackground(byte r, byte g, byte b) => $"\u001b[48;2;{r};{g};{b}m";
    
    // Reset all colors and styles
    public static string Reset => "\u001b[0m";
    
    // Convenience method for named colors
    public static string Colorize(string text, AnsiConsoleColor color) => 
        $"\u001b[{(int)color}m{text}{Reset}";
}*/
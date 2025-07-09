namespace Model.GenerateView;


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

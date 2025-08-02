namespace Client;
using System.Runtime.InteropServices;

public static class ConsoleManager
{
    const int STD_OUTPUT_HANDLE = -11;
    const int STD_INPUT_HANDLE = -10;
    const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
    const uint ENABLE_PROCESSED_INPUT = 0x0001;
    const uint ENABLE_LINE_INPUT = 0x0002;
    const uint ENABLE_ECHO_INPUT = 0x0004;
    const uint ENABLE_MOUSE_INPUT = 0x0010;
    
    
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    
    public static void ConfigureConsole()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            EnableWindowsAnsi();
            ConfigureWindowsInput();
        }
    }
    
    public static void RestoreConsole()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            RestoreWindowsInput();
        }
    }

    private static void EnableWindowsAnsi()
    {
        var stdout = GetStdHandle(STD_OUTPUT_HANDLE);
        GetConsoleMode(stdout, out var mode);
        mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        SetConsoleMode(stdout, mode);
    }

    private static void ConfigureWindowsInput()
    {
        var stdin = GetStdHandle(STD_INPUT_HANDLE);
        GetConsoleMode(stdin, out var mode);
        
        mode = ENABLE_PROCESSED_INPUT;
        SetConsoleMode(stdin, mode);
    }

    private static void RestoreWindowsInput()
    {
        var stdin = GetStdHandle(STD_INPUT_HANDLE);
        GetConsoleMode(stdin, out var mode);
        
        mode = ENABLE_PROCESSED_INPUT | 
               ENABLE_LINE_INPUT | 
               ENABLE_ECHO_INPUT;
        SetConsoleMode(stdin, mode);
    }
    
}
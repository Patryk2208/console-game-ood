using System.Text;

namespace Project_oob.Display;

/*public class ConsoleDisplayGame(int w, int h) : IDisplay
{
    public void Display()
    {
        var a = new StringBuilder();
        for (int i = 0; i < Width; i++)
        {
            a.Append('_');
        }
        Console.Write(a.ToString());
        for (int i = 1; i < Height - 1; i++)
        {
            a.Append(@$"\033[{i};1");
            //Console.SetCursorPosition(0, i);
            a.Append("|");
            //Console.Write('|');
            a.Append(@$"\033[{i};{Width}");
            //Console.SetCursorPosition(Width - 1, i);
            a.Append("|\n");
            /*Console.Write('|');
            Console.WriteLine();#1#
        }

        //Console.Write(a.ToString());
    }
}*/
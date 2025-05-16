namespace RPG_ood.Model.Game;

public class Logs
{
    private const int LogCapacity = 10;
    private const int logLength = 80;
    public Queue<string> LogMessgaes { get; set; } = new(LogCapacity);

    public void AddLogMessage(string message)
    {
        if (message.Length < logLength)
        {
            message = message.PadRight(logLength);
        }
        else if (message.Length > logLength)
        {
            message = message[..logLength];
        }
        if (LogMessgaes.Count < LogCapacity)
        {
            LogMessgaes.Enqueue(message);
        }
        else
        {
            LogMessgaes.Dequeue();
            LogMessgaes.Enqueue(message);
        }
    }
    
}
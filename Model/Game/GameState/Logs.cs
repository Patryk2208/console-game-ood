namespace Model.Game.GameState;

public class Logs
{
    private const int LogCapacity = 10;
    private const int LogLength = 80;
    public Dictionary<long, Queue<string>> LogMessages { get; set; } = new();

    public void AddPlayerLogs(long playerId)
    {
        LogMessages.TryAdd(playerId, new Queue<string>(LogCapacity));
    }

    public void RemovePlayerLogs(long playerId)
    {
        LogMessages.Remove(playerId);
    }

    public void AddCommonLogMessage(string message)
    {
        foreach (var receiver in LogMessages.Keys)
        {
            AddLogMessage(receiver, message);
        }
    }
    
    public void AddLogMessage(long playerId, string message)
    {
        if (!LogMessages.ContainsKey(playerId))
        {
            return;
        }
        if (message.Length < LogLength)
        {
            message = message.PadRight(LogLength);
        }
        else if (message.Length > LogLength)
        {
            message = message[..LogLength];
        }
        if (LogMessages[playerId].Count >= LogCapacity)
        {
            LogMessages[playerId].Dequeue();
        }

        LogMessages[playerId].Enqueue(message);
    }
    
}
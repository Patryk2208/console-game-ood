namespace Model.Communication.Snapshots;

public class LogsSnapshot
{
    public IEnumerable<string> LogMessages { get; set; }

    public LogsSnapshot(IEnumerable<string> logMessages)
    {
        LogMessages = logMessages;
    }
}
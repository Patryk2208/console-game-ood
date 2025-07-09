using Model.Communication.Snapshots;

namespace Model.RelativeGameState;

public class RelativeLogs
{
    public IEnumerable<string> LogMessages { get; set; }

    public RelativeLogs(LogsSnapshot logs)
    {
        LogMessages = logs.LogMessages;
    }
}
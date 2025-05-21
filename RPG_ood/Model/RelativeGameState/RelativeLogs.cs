using RPG_ood.Communication.Snapshots;

namespace RPG_ood.Model.RelativeGameState;

public class RelativeLogs
{
    public IEnumerable<string> LogMessages { get; set; }

    public RelativeLogs(LogsSnapshot logs)
    {
        LogMessages = logs.LogMessages;
    }
}
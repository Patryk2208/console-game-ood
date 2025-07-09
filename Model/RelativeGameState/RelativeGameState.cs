using Model.Game.Beings;

namespace Model.RelativeGameState;

public class RelativeGameState
{
    public long LastSyncMoment { get; set; }
    public Player Player { get; set; }
    public List<string> AppliedEffects { get; set; }
    public RelativeRoomState CurrentRelativeRoom { get; set; }
    public RelativeLogs CurrentRelativeLogs { get; set; }
    public MvcSynchronization Sync { get; set; }

    public RelativeGameState(MvcSynchronization sync)
    {
        Sync = sync;
        LastSyncMoment = -1;
    }
    
}
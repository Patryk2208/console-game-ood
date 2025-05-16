using RPG_ood.App;
using RPG_ood.Map;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game;
using RPG_ood.Model.Game.Beings;

namespace RPG_ood.Model.GameSnapshot;

public class RelativeGameState
{
    public long LastSyncMoment { get; set; }
    public Player Player { get; set; }
    public RelativeRoomState CurrentRelativeRoom { get; set; }
    public Logs ThisSnapshotLogs { get; set; }
    public MvcSynchronization Sync { get; set; }

    public RelativeGameState(MvcSynchronization sync)
    {
        Sync = sync;
        LastSyncMoment = -1;
    }
    
}
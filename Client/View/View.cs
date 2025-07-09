using Model;
using Model.Game.Map;
using Model.RelativeGameState;

namespace Client.View;

public class View(RelativeGameState relativeGameState, MvcSynchronization sync)
{
    private RelativeGameState State { get; init; } = relativeGameState;

    private MvcSynchronization Sync { get; init; } = sync;

    public void Prepare()
    {
        var builder = new RoomInstructionBuilder();
        var dir = new PlayableMaze(builder);
        dir.Build();
        var displaySystem = Display.GetInstance();
        displaySystem.PrepareGame();
        displaySystem.DisplayInstructions(builder.GetResult());
    }

    public void Refresh()
    {
        var displaySystem = Display.GetInstance();
        
        displaySystem.DisplayFrame(State);
        displaySystem.DisplayMapInfo(State);
        displaySystem.DisplayLog(State.CurrentRelativeLogs);

        displaySystem.RefreshRoom(State.CurrentRelativeRoom);
        displaySystem.RefreshItems(State.CurrentRelativeRoom);
        displaySystem.RefreshBeings(State.CurrentRelativeRoom);

        displaySystem.RefreshItemsOnPosition(State);
        displaySystem.RefreshEnemiesNearby(State);
        displaySystem.RefreshPlayerInfo(State);

        displaySystem.RefreshPlayers(State);
        
        displaySystem.DisplayGame();
    }

    public void Cleanup()
    {
        var displaySystem = Display.GetInstance();
        displaySystem.CleanupGame();
    }
    
    public async Task RunDisplay()
    {
        /*Task.Run(
            () =>
            {
                Display displaySystem = Display.GetInstance();
                
                displaySystem.PrepareGame();
                displaySystem.DisplayInstructions(State.CurrentRelativeRoom.RoomInstruction);
                while (Sync.ShouldExitView == false)
                {
                    Thread.Sleep(17);
                    Sync.GameMutex.WaitOne();
                    
                    displaySystem.DisplayFrame(State);
                    displaySystem.DisplayMapInfo(State);
                    
                    displaySystem.RefreshRoom(State.CurrentRelativeRoom);
                    displaySystem.RefreshItems(State.CurrentRelativeRoom);
                    displaySystem.RefreshBeings(State.CurrentRelativeRoom);
                    
                    displaySystem.RefreshItemsOnPosition(State);
                    displaySystem.RefreshEnemiesNearby(State);
                    displaySystem.RefreshPlayerInfo(State);
                    
                    displaySystem.RefreshPlayers(State.Player);
                    
                    displaySystem.DisplayGame();
                    
                    Sync.GameMutex.ReleaseMutex();
                }
                displaySystem.CleanupGame();
            });*/

        try
        {

            Display displaySystem = Display.GetInstance();

            displaySystem.PrepareGame();
            //displaySystem.DisplayInstructions(State.CurrentRelativeRoom.RoomInstruction);
            while (Sync.ShouldExitView == false)
            {
                await Task.Delay(17);
                Sync.GameMutex.WaitOne();

                if (State.LastSyncMoment == -1)
                {
                    Sync.GameMutex.ReleaseMutex();
                    continue;
                }

                displaySystem.DisplayFrame(State);
                displaySystem.DisplayMapInfo(State);

                displaySystem.RefreshRoom(State.CurrentRelativeRoom);
                displaySystem.RefreshItems(State.CurrentRelativeRoom);
                displaySystem.RefreshBeings(State.CurrentRelativeRoom);

                displaySystem.RefreshItemsOnPosition(State);
                displaySystem.RefreshEnemiesNearby(State);
                displaySystem.RefreshPlayerInfo(State);

                displaySystem.RefreshPlayers(State);

                displaySystem.DisplayGame();

                Sync.GameMutex.ReleaseMutex();
            }

            displaySystem.CleanupGame();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
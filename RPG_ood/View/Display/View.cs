using System.Runtime.CompilerServices;
using RPG_ood.App;
using RPG_ood.Map;
using RPG_ood.Model.Game;
using RPG_ood.Model.RelativeGameState;

namespace RPG_ood.View.Display;

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


public enum AnsiConsoleColor
{
    // Standard foreground colors
    Black = 30,
    Red = 31,
    Green = 32,
    Yellow = 33,
    Blue = 34,
    Magenta = 35,
    Cyan = 36,
    White = 37,
    
    // Bright foreground colors
    BrightBlack = 90,
    BrightRed = 91,
    BrightGreen = 92,
    BrightYellow = 93,
    BrightBlue = 94,
    BrightMagenta = 95,
    BrightCyan = 96,
    BrightWhite = 97,
    
    // Standard background colors
    BgBlack = 40,
    BgRed = 41,
    BgGreen = 42,
    BgYellow = 43,
    BgBlue = 44,
    BgMagenta = 45,
    BgCyan = 46,
    BgWhite = 47,
    
    // Bright background colors
    BgBrightBlack = 100,
    BgBrightRed = 101,
    BgBrightGreen = 102,
    BgBrightYellow = 103,
    BgBrightBlue = 104,
    BgBrightMagenta = 105,
    BgBrightCyan = 106,
    BgBrightWhite = 107
}
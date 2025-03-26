using System.Linq;
using RPG_ood.Beings;
using RPG_ood.Effects;
using RPG_ood.Items;
using RPG_ood.Map;

namespace RPG_ood.Game;

public abstract class GameState
{
    //map section
    public Player Player { get; set; }
    public World World { get; protected set; }
    public Map.Map CurrentMap { get; protected set; }
    public Room CurrentRoom { get; protected set; }
    
    //display
    //non-input state changes
}

public class SinglePlayerGameState : GameState
{
    private CancellationTokenSource _cts;
    private Mutex _mutex;
    public SinglePlayerGameState(string userName, Mutex mutex, CancellationTokenSource cts)
    {
        _mutex = mutex;
        _cts = cts;
        Player = new Player(userName);
        World = new World();
        World.Maps.Add("Main", new Map.Map("Main", 1));
        CurrentMap = World.Maps["Main"];
        var builder = new RoomBuilder("Underworld", 40, 20);
        var instructionBuilder = new RoomInstructionBuilder();
        //var director = new SimpleRaggedMaze(builder);
        var director = new PlayableMaze(builder);
        //var director = new TestMaze(builder);
        director.Build();
        director.SwitchBuilders(instructionBuilder);
        director.Build();
        World.Maps["Main"].Rooms[0] = builder.GetResult();
        CurrentRoom = World.Maps["Main"].Rooms[0];
        CurrentRoom.RoomInstruction = instructionBuilder.GetResult();
    }

    //display
    public async Task RunDisplay()
    {
        Display.Display displaySystem = Display.Display.GetInstance();
        displaySystem.PrepareGame();
        displaySystem.DisplayInstructions(CurrentRoom.RoomInstruction);
        await Task.Run(
            () =>
            {
                while (_cts.Token.IsCancellationRequested == false)
                {
                    Thread.Sleep(17);
                    _mutex.WaitOne();
                    
                    displaySystem.DisplayFrame(this);
                    displaySystem.DisplayMapInfo(this);
                    
                    displaySystem.RefreshRoom(CurrentRoom);
                    displaySystem.RefreshItems(CurrentRoom);
                    displaySystem.RefreshBeings(CurrentRoom);
                    
                    displaySystem.RefreshItemsOnPosition(this);
                    displaySystem.RefreshEnemiesNearby(this);
                    displaySystem.RefreshPlayerInfo(this);
                    displaySystem.RefreshPlayers(Player);
                    
                    displaySystem.DisplayGame();
                    
                    _mutex.ReleaseMutex();
                }
            }, _cts.Token);
        displaySystem.CleanupGame();
    }
    
    //non-input state changes
}
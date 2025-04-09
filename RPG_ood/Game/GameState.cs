using System.Linq;
using RPG_ood.Beings;
using RPG_ood.Effects;
using RPG_ood.Items;
using RPG_ood.Map;
using RPG_ood.Input;

namespace RPG_ood.Game;

public abstract class GameState
{
    //map section
    public Player Player { get; set; }
    public World World { get; protected init; }
    public Map.Map CurrentMap { get; protected set; }
    public Room CurrentRoom { get; protected set; }
    protected Logs Logs { get; init; }
    public abstract void RunGame();
    public abstract void QuitGame();

    //display
    //non-input state changes
    protected MomentChangedEvent MomentChangedEvent { get; init; }
    protected int MomentDurationMilliseconds { get; init; }
}

public class SinglePlayerGameState : GameState
{
    private CancellationTokenSource _cts;
    private Mutex _mutex;
    public SinglePlayerGameState(string userName, Mutex mutex, CancellationTokenSource cts, Input.Input input)
    {
        _mutex = mutex;
        _cts = cts;
        MomentChangedEvent = new MomentChangedEvent();
        MomentDurationMilliseconds = 100;
        Logs = new Logs();
        Player = new Player(userName);
        World = new World();
        World.Maps.Add("Main", new Map.Map("Main", 1));
        CurrentMap = World.Maps["Main"];
        var builder = new RoomBuilder("Underworld", 40, 20);
        var instructionBuilder = new RoomInstructionBuilder();
        var inputHandlingBuilder = new InputHandlingBuilder(this, Logs);
        
        //var director = new SimpleRaggedMaze(builder);
        var director = new PlayableMaze(builder);
        //var director = new TestMaze(builder);
        director.Build();
        director.SwitchBuilders(instructionBuilder);
        director.Build();
        director.SwitchBuilders(inputHandlingBuilder);
        director.Build();
        
        World.Maps["Main"].Rooms[0] = builder.GetResult();
        CurrentRoom = World.Maps["Main"].Rooms[0];
        CurrentRoom.RoomInstruction = instructionBuilder.GetResult();
        
        MomentChangedEvent.AddObserver(Player.Name, Player);
        foreach (var being in CurrentRoom.Beings)
        {
            MomentChangedEvent.AddObserver(being.Name, being);
        }
        
        input.ChainOfResponsibilityHandler = inputHandlingBuilder.GetResult();
    }

    public override void RunGame()
    {
        while (_cts.Token.IsCancellationRequested == false)
        {
            Thread.Sleep(MomentDurationMilliseconds);
            _mutex.WaitOne();
            MomentChangedEvent.NotifyObservers(this);
            _mutex.ReleaseMutex();
        }
    }

    public override void QuitGame()
    {
        _cts.Cancel();
        MomentChangedEvent.ClearObservers(); //Todo Incorrect Exit
    }
    
    //display
    public void RunDisplay()
    {
        Task.Run(
            () =>
            {
                Display.Display displaySystem = Display.Display.GetInstance();
                displaySystem.PrepareGame();
                displaySystem.DisplayInstructions(CurrentRoom.RoomInstruction);
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
                displaySystem.CleanupGame();
            }, _cts.Token);
    }
}
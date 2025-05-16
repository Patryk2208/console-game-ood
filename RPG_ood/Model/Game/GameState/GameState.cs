using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using RPG_ood.App;
using RPG_ood.Communication.Snapshots;
using RPG_ood.Controller.Input;
using RPG_ood.Map;
using RPG_ood.Model.Beings;
using RPG_ood.Communication.Snapshots;
using RPG_ood.Model.Game.Beings;

namespace RPG_ood.Model.Game.GameState;

public class GameState
{
    public Dictionary<long, Player> Players { get; set; }
    private Dictionary<long, Channel<Communication.Snapshots.GameSnapshot>> PlayerChannels { get; set; }
    public World World { get; protected init; }
    public RPG_ood.Map.Map CurrentMap { get; protected set; }
    public Room CurrentRoom { get; protected set; }
    public Logs Logs { get; init; }
    
    
    public MomentChangedEvent MomentChangedEvent { get; init; }
    private int MomentDurationMilliseconds { get; init; }
    public long CurrentMoment { get; private set; }
    
    
    private MvcSynchronization Sync { get; init; }
    public GameState(MvcSynchronization sync, Input.Input input, Dictionary<long, Channel<Communication.Snapshots.GameSnapshot>> playerChannels)
    {
        Sync = sync;
        MomentChangedEvent = new MomentChangedEvent();
        MomentDurationMilliseconds = 100;
        Logs = new Logs();
        Players = new();
        PlayerChannels = playerChannels;
        World = new World();
        World.Maps.Add("Main", new RPG_ood.Map.Map("Main", 1));
        CurrentMap = World.Maps["Main"];
        var builder = new RoomBuilder("Underworld", 40, 20);
        var instructionBuilder = new RoomInstructionBuilder();
        var inputHandlingBuilder = new InputHandlingBuilder(this);
        
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
        foreach (var p in Players.Values)
        {
            CurrentRoom.Players.Add(p);
            MomentChangedEvent.AddObserver(p.Name, p);
        }
        foreach (var being in CurrentRoom.Beings)
        {
            MomentChangedEvent.AddObserver(being.Name, being);
        }
        
        input.ChainOfResponsibilityHandler = inputHandlingBuilder.GetResult();
    }

    public async Task RunGame()
    {
        while (Sync.ShouldExitModel == false)
        {
            await Task.Delay(MomentDurationMilliseconds);
            Sync.GameMutex.WaitOne();
            
            await BroadcastStates();
            
            MomentChangedEvent.NotifyObservers(this, 0);
            ++CurrentMoment;
            
            Sync.GameMutex.ReleaseMutex();
        }
    }

    public bool IsAnyoneAlive()
    {
        var alive = false;
        foreach (var p in Players.Values)
        {
            alive |= !p.IsDead;
        }
        return alive;
    }
    public void QuitGame()
    {
        Sync.ShouldAllExit = true;
        MomentChangedEvent.ClearObservers(); //Todo Incorrect Exit
    }
    
    //fights
    public Player? ChoosePlayerToFight(Player p, float radius = 2.5f)
    {
        var random = new Random();
        var playersAdjacent = CurrentRoom
            .GetPlayersNearby(p.Pos, radius)
            .ToList();
        return playersAdjacent.Count == 0 ? null : playersAdjacent[random.Next(playersAdjacent.Count)];
    }
    public IEnemy? ChooseEnemyToFight(Player p, float radius = 2.5f)
    {
        var random = new Random();
        var enemiesAdjacent = CurrentRoom
            .GetBeingsNearby(p.Pos, radius)
            .Where(e => e.CanFight() != null)
            .Select(e => e.CanFight()!)
            .ToList();
        return enemiesAdjacent.Count == 0 ? null : enemiesAdjacent[random.Next(enemiesAdjacent.Count)];
    }

    public void AddPlayer(long id)
    {
        var count = Players.Count;
        var player = new Player($"{count}");
        player.Id = id;
        Players.Add(id, player);
        CurrentRoom.Players.Add(player);
        MomentChangedEvent.AddObserver(id.ToString(), player);
        var possibleSpawnPositions = new List<(int, int)>();
        for(int i = 0; i < CurrentRoom.Height; ++i)
        {
            for (int j = 0; j < CurrentRoom.Width; ++j)
            {
                if (CurrentRoom.Elements[i, j].OnStandable)
                {
                    possibleSpawnPositions.Add((i, j));
                }
            }
        }
        var spawnCoords = possibleSpawnPositions[new Random().Next(possibleSpawnPositions.Count)];
        Players[id].Pos = new Position(spawnCoords.Item1, spawnCoords.Item2);
        var channel = Channel.CreateUnbounded<Communication.Snapshots.GameSnapshot>();
        PlayerChannels.Add(id, channel);
    }

    public void RemovePlayer(long id)
    {
        var player = Players[id];
        CurrentRoom.Players.Remove(player);
        //CurrentRoom.Elements[player.Pos.X, player.Pos.Y].OnStandable = true;
        PlayerChannels.Remove(id);
        Players.Remove(id);
        MomentChangedEvent.RemoveObserver(player.Name, player);
    }
    
    //Communication and state synchronization
    /*public string SerializeInitialPlayerState(long playerId)
    {
        var tempStateSnapshot = new Communication.Snapshots.GameSnapshot(this, playerId);
        return JsonSerializer.Serialize(tempStateSnapshot, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new GameSnapshotJsonConverter(),
                new RoomSnapshotJsonConverter()
            }
        });
    }*/

    private async Task BroadcastStates()
    {
        foreach (var channel in PlayerChannels)
        {
            var relativeState = new Communication.Snapshots.GameSnapshot(this, channel.Key);
            await channel.Value.Writer.WriteAsync(relativeState);
        }
    }
}
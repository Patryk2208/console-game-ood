using System.Text.Json;
using System.Text.Json.Serialization;
using Model.Game.Beings;
using Model.Game.GameState;

namespace Model.Communication.Snapshots;

[Serializable]
[JsonConverter(typeof(GameSnapshotJsonConverter))]
public class GameSnapshot
{
    public long SyncMoment { get; set; }
    public Player Player { get; private set; }
    public List<string> AppliedEffects { get; private set; }
    public RoomSnapshot CurrentRoomSnapshot { get; private set; }
    public LogsSnapshot Logs { get; private set; }

    public GameSnapshot(GameState gameState, long playerId)
    {
        SyncMoment = gameState.CurrentMoment;
        Player = gameState.Players[playerId];
        AppliedEffects = Player.MomentChangedEvent.Names.ToList();
        CurrentRoomSnapshot = new RoomSnapshot(gameState.CurrentRoom);
        Logs = new LogsSnapshot(gameState.Logs.LogMessages[playerId]);
    }

    [JsonConstructor]
    public GameSnapshot(long syncMoment, Player player, RoomSnapshot currentRoomSnapshot, LogsSnapshot logs, List<string> appliedEffects)
    {
        SyncMoment = syncMoment;
        Player = player;
        AppliedEffects = appliedEffects;
        CurrentRoomSnapshot = currentRoomSnapshot;
        Logs = logs;
    }
}

public class GameSnapshotJsonConverter : JsonConverter<GameSnapshot>
{
    public override GameSnapshot? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            var syncMoment = root.GetProperty("SyncMoment").GetInt64();
            var player = root.GetProperty("Player").Deserialize<Player>();
            var appliedEffects = root.GetProperty("AppliedEffects").Deserialize<List<string>>();
            var currentRoomSnapshot = root.GetProperty("CurrentRoomSnapshot").Deserialize<RoomSnapshot>();
            var logs = root.GetProperty("Logs").Deserialize<LogsSnapshot>();

            return new GameSnapshot(syncMoment, player, currentRoomSnapshot, logs, appliedEffects);
        }
    }

    public override void Write(Utf8JsonWriter writer, GameSnapshot value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("SyncMoment", value.SyncMoment);
        writer.WritePropertyName("Player");
        JsonSerializer.Serialize(writer, value.Player, options);
        writer.WritePropertyName("AppliedEffects");
        JsonSerializer.Serialize(writer, value.AppliedEffects, options);
        writer.WritePropertyName("CurrentRoomSnapshot");
        JsonSerializer.Serialize(writer, value.CurrentRoomSnapshot, options);
        writer.WritePropertyName("Logs");
        JsonSerializer.Serialize(writer, value.Logs, options);
        writer.WriteEndObject();
    }
}
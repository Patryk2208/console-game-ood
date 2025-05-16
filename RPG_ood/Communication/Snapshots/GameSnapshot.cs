using System.Text.Json;
using System.Text.Json.Serialization;
using RPG_ood.Map;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game;
using RPG_ood.Model.Game.Beings;
using RPG_ood.Model.Game.GameState;
using RPG_ood.Model.GameSnapshot;

namespace RPG_ood.Communication.Snapshots;

[Serializable]
[JsonConverter(typeof(GameSnapshotJsonConverter))]
public class GameSnapshot
{
    public long SyncMoment { get; set; }
    public Player Player { get; private set; }
    public RoomSnapshot CurrentRoomSnapshot { get; private set; }

    public GameSnapshot(GameState gameState, long playerID)
    {
        SyncMoment = gameState.CurrentMoment;
        Player = gameState.Players[playerID];
        CurrentRoomSnapshot = new RoomSnapshot(gameState.CurrentRoom);
    }

    [JsonConstructor]
    public GameSnapshot(long syncMoment, Player player, RoomSnapshot currentRoomSnapshot)
    {
        SyncMoment = syncMoment;
        Player = player;
        CurrentRoomSnapshot = currentRoomSnapshot;
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
            var currentRoomSnapshot = root.GetProperty("CurrentRoomSnapshot").Deserialize<RoomSnapshot>();

            return new GameSnapshot(syncMoment, player, currentRoomSnapshot);
        }
    }

    public override void Write(Utf8JsonWriter writer, GameSnapshot value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("SyncMoment", value.SyncMoment);
        writer.WritePropertyName("Player");
        JsonSerializer.Serialize(writer, value.Player, options);
        writer.WritePropertyName("CurrentRoomSnapshot");
        JsonSerializer.Serialize(writer, value.CurrentRoomSnapshot, options);
        writer.WriteEndObject();
    }
}
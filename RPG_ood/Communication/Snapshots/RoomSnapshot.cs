using System.Text.Json;
using System.Text.Json.Serialization;
using RPG_ood.Map;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game;
using RPG_ood.Model.Game.Beings;
using RPG_ood.Model.Game.Items;
using RPG_ood.Model.Items;

namespace RPG_ood.Communication.Snapshots;

[Serializable]
[JsonConverter(typeof(RoomSnapshotJsonConverter))]
public class RoomSnapshot
{
    public string Name { get; protected set; }
    public int Width { get; }
    public int Height { get; }
    [JsonIgnore]
    public MapElement[,] Elements { get; protected set; }
    public List<IBeing> Beings { get; protected set; }
    public List<IItem> Items { get; protected set; }
    public List<PlayerSnapshot> Players { get; protected set; }

    public RoomSnapshot(Room room)
    {
        Name = room.Name;
        Width = room.Width;
        Height = room.Height;
        Elements = room.Elements;
        Beings = room.Beings;
        Items = room.Items;
        Players = new();
        foreach (var p in room.Players)
        {
            Players.Add(new PlayerSnapshot(p));
        }
    }

    [JsonConstructor]
    public RoomSnapshot(string name, int width, int height, MapElement[,] elements, List<IBeing> beings,
        List<IItem> items, List<PlayerSnapshot> players)
    {
        Name = name;
        Width = width;
        Height = height;
        Elements = elements;
        Beings = beings;
        Items = items;
        Players = players;
    }
}

public class RoomSnapshotJsonConverter : JsonConverter<RoomSnapshot>
{
    public override RoomSnapshot? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            var name = root.GetProperty("Name").GetString()!;
            var width = root.GetProperty("Width").GetInt32();
            var height = root.GetProperty("Height").GetInt32(); 
            var elements = new MapElement[height, width];
            var rawElems = root.GetProperty("Elements").EnumerateArray().ToArray();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    elements[i, j] = rawElems[i * width + j].Deserialize<MapElement>(options);
                }
            }
            var beings = root.GetProperty("Beings").Deserialize<List<IBeing>>();
            var items = root.GetProperty("Items").Deserialize<List<IItem>>();
            var players = root.GetProperty("Players").Deserialize<List<PlayerSnapshot>>();
            
            return new RoomSnapshot(name, width, height, elements, beings, items, players);
        }
    }

    public override void Write(Utf8JsonWriter writer, RoomSnapshot value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", value.GetType().Name);
        writer.WriteString("Name", value.Name);
        writer.WriteNumber("Width", value.Width);
        writer.WriteNumber("Height", value.Height);
        
        writer.WriteStartArray("Elements");
        for (int i = 0; i < value.Height; i++)
        {
            for (int j = 0; j < value.Width; j++)
            {
                JsonSerializer.Serialize(writer, value.Elements[i, j], options);
                //JsonSerializer.Serialize(writer, value.Elements[i, j].GetType().Name[0], options);
            }
        }
        writer.WriteEndArray();
        
        writer.WriteStartArray("Beings");
        foreach (var being in value.Beings)
        {
            JsonSerializer.Serialize(writer, being, options);
        }

        writer.WriteEndArray();

        writer.WriteStartArray("Items");
        foreach (var item in value.Items)
        {
            JsonSerializer.Serialize(writer, item, options);
        }

        writer.WriteEndArray();

        writer.WriteStartArray("Players");
        foreach (var player in value.Players)
        {
            JsonSerializer.Serialize(writer, player, options);
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}
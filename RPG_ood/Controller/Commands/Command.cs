using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPG_ood.Commands;

[Serializable]
[JsonConverter(typeof(CommandJsonConverter))]
public class Command
{
    public ConsoleKeyInfo KeyInfo { get; set; }
    public long PlayerId { get; set; }

    [JsonConstructor]
    public Command(ConsoleKeyInfo keyInfo, long playerId)
    {
        KeyInfo = keyInfo;
        PlayerId = playerId;
    }
}

public class CommandJsonConverter : JsonConverter<Command>
{
    public override Command? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            var keyInfo = root.GetProperty("KeyInfo");
            var keyChar = keyInfo.GetProperty("KeyChar").Deserialize<char>();
            var consoleKey = Enum.Parse<ConsoleKey>(keyInfo.GetProperty("ConsoleKey").GetString());
            var mods = Enum.Parse<ConsoleModifiers>(keyInfo.GetProperty("Modifiers").ToString());
            var playerId = root.GetProperty("PlayerId").GetInt64();
            return new Command(
                new ConsoleKeyInfo(keyChar, consoleKey, 
                    mods == ConsoleModifiers.Shift, 
                    mods == ConsoleModifiers.Alt, 
                    mods == ConsoleModifiers.Control),
                playerId);
        }
    }

    public override void Write(Utf8JsonWriter writer, Command value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("KeyInfo");
        writer.WriteStartObject();
        writer.WritePropertyName("KeyChar");
        writer.WriteStringValue(value.KeyInfo.KeyChar.ToString());
        writer.WritePropertyName("ConsoleKey");
        writer.WriteStringValue(value.KeyInfo.Key.ToString());
        writer.WritePropertyName("Modifiers");
        writer.WriteStringValue(value.KeyInfo.Modifiers .ToString());
        writer.WriteEndObject();
        writer.WritePropertyName("PlayerId");
        writer.WriteNumberValue(value.PlayerId);
        writer.WriteEndObject();
    }
}
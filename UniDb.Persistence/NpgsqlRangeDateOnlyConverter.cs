using System.Text.Json;
using System.Text.Json.Serialization;
using NpgsqlTypes;

// mapster or mapperly or automapper
namespace UniDb.Persistence;

public class NpgsqlRangeDateOnlyConverter : JsonConverter<NpgsqlRange<DateOnly>>
{
    public override NpgsqlRange<DateOnly> Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var obj = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
        var lowerBound = obj.GetProperty("lowerBound").ToString();
        var upperBound = obj.GetProperty("upperBound").ToString();
        return new NpgsqlRange<DateOnly>(DateOnly.Parse(lowerBound), DateOnly.Parse(upperBound));
    }

    public override void Write(Utf8JsonWriter writer, NpgsqlRange<DateOnly> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("lowerBound", value.LowerBound.ToString());
        writer.WriteString("upperBound", value.UpperBound.ToString());
        writer.WriteEndObject();
    }
}
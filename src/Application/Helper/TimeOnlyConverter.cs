using System.Text.Json.Serialization;
using System.Text.Json;

namespace Space.Application.Helper;

public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    private readonly string _serializationFormat;
    public TimeOnlyConverter() : this(null)
    {
    }
    public TimeOnlyConverter(string? serializationFormat)
    {
        _serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
    }
    public override TimeOnly Read(ref Utf8JsonReader reader,
                            Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return TimeOnly.Parse(value!);
    }
    public override void Write(Utf8JsonWriter writer, TimeOnly value,
                                        JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(_serializationFormat));
}

public class TimeOnlyNullableConverter : JsonConverter<TimeOnly?>
{
    private readonly string _serializationFormat;
    public TimeOnlyNullableConverter() : this(null)
    {
    }
    public TimeOnlyNullableConverter(string? serializationFormat)
    {
        _serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
    }
    public override TimeOnly? Read(ref Utf8JsonReader reader,
    Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value == null ? null : TimeOnly.Parse(value);
    }
    public override void Write(Utf8JsonWriter writer, TimeOnly? value,
                                    JsonSerializerOptions options)
    => writer.WriteStringValue(value?.ToString(_serializationFormat));
}
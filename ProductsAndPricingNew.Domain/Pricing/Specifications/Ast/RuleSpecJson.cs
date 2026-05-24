using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

/// <summary>
/// Serializes the specification AST to / from the JSON persisted in
/// <c>PricingRule.ScriptJson</c>.
/// </summary>
public static class RuleSpecJson
{
    public static JsonSerializerOptions Options { get; } = CreateOptions();

    public static string Serialize(RuleNode node) => JsonSerializer.Serialize(node, Options);

    public static RuleNode Deserialize(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("Rule script JSON is required.", nameof(json));

        return JsonSerializer.Deserialize<RuleNode>(json, Options)
               ?? throw new JsonException("Rule script JSON deserialized to null.");
    }

    private static JsonSerializerOptions CreateOptions()
    {
        JsonSerializerOptions options = new() { WriteIndented = false };
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new RuleValueJsonConverter());
        return options;
    }
}

internal sealed class RuleValueJsonConverter : JsonConverter<RuleValue>
{
    public override RuleValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;

        string typeName = root.GetProperty("Type").GetString()
                           ?? throw new JsonException("RuleValue.Type is missing.");
        RuleValueType type = Enum.Parse<RuleValueType>(typeName, ignoreCase: true);
        JsonElement value = root.GetProperty("Value");

        return type switch
        {
            RuleValueType.String => RuleValue.String(RequireString(value)),
            RuleValueType.Enum => RuleValue.Enum(RequireString(value)),
            RuleValueType.Int => RuleValue.Int(value.GetInt64()),
            RuleValueType.Decimal => RuleValue.Decimal(value.GetDecimal()),
            RuleValueType.Bool => RuleValue.Bool(value.GetBoolean()),
            RuleValueType.Date => RuleValue.Date(
                DateOnly.ParseExact(RequireString(value), "yyyy-MM-dd", CultureInfo.InvariantCulture)),
            _ => throw new JsonException($"Unknown RuleValueType: {type}.")
        };
    }

    public override void Write(Utf8JsonWriter writer, RuleValue value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Type", value.Type.ToString());
        writer.WritePropertyName("Value");

        switch (value.Type)
        {
            case RuleValueType.String:
            case RuleValueType.Enum:
                writer.WriteStringValue((string)value.Value);
                break;
            case RuleValueType.Int:
                writer.WriteNumberValue((long)value.Value);
                break;
            case RuleValueType.Decimal:
                writer.WriteNumberValue((decimal)value.Value);
                break;
            case RuleValueType.Bool:
                writer.WriteBooleanValue((bool)value.Value);
                break;
            case RuleValueType.Date:
                writer.WriteStringValue(((DateOnly)value.Value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                break;
            default:
                throw new JsonException($"Unknown RuleValueType: {value.Type}.");
        }

        writer.WriteEndObject();
    }

    private static string RequireString(JsonElement element) =>
        element.GetString() ?? throw new JsonException("RuleValue.Value is null.");
}

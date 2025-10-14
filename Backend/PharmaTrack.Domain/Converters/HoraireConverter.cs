using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using PharmaTrack.Domain.enums;

namespace PharmaTrack.Domain.Converters;


// FIX: Custom JSON Converter to handle string-to-enum conversion reliably
public class HoraireConverter : JsonConverter<Horaire>
{
    public override Horaire Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            // Fallback for unexpected token types (e.g., if it was serialized as a number)
            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt32(out int intValue) && Enum.IsDefined(typeof(Horaire), intValue))
                {
                    return (Horaire)intValue;
                }
            }
            throw new JsonException($"Expected string for Horaire, but found {reader.TokenType}.");
        }

        string value = reader.GetString()?.Trim();
        
        // Case-insensitive conversion logic
        if (string.Equals(value, "Jour", StringComparison.OrdinalIgnoreCase))
        {
            return Horaire.Jour;
        }
        if (string.Equals(value, "Nuit", StringComparison.OrdinalIgnoreCase))
        {
            return Horaire.Nuit;
        }

        throw new JsonException($"Unable to convert JSON string '{value}' to Horaire enum.");
    }

    public override void Write(Utf8JsonWriter writer, Horaire value, JsonSerializerOptions options)
    {
        // When writing back to JSON, just use the string name of the enum
        writer.WriteStringValue(value.ToString());
    }
}
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using PharmaTrack.Domain.enums;

namespace PharmaTrack.Domain.Converters;

public class PharmacieTypeConverter : JsonConverter<PharmacieType>
{
    public override PharmacieType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected string for PharmacieType, but found {reader.TokenType}.");
        }

        string value = reader.GetString()?.Trim();
        
        // Case-insensitive conversion logic
        if (string.Equals(value, "Pharmacie", StringComparison.OrdinalIgnoreCase))
        {
            return PharmacieType.Pharmacie;
        }
        if (string.Equals(value, "ParaPharmacie", StringComparison.OrdinalIgnoreCase))
        {
            return PharmacieType.ParaPharamacie;
        }
        // Add logic for other types here if you expand the enum, e.g.:
        // if (string.Equals(value, "Parapharmacie", StringComparison.OrdinalIgnoreCase))
        // {
        //     return PharmacieType.Parapharmacie;
        // }

        throw new JsonException($"Unable to convert JSON string '{value}' to PharmacieType enum.");
    }

    public override void Write(Utf8JsonWriter writer, PharmacieType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
// NEW: Custom JSON Converter to handle string-to-enum conversion for PharmacieType
    }
}

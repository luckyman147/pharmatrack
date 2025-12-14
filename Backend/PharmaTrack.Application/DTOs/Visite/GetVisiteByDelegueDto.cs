using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.DTOs.Visite;

public class GetVisiteByDelegueDto
{
    public Guid DelegueId { get; set; }
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? startDate { get; set; }
    public string? byName { get; set; }

    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 12;
    public string? zone { get; set; }
    public string? Gouvernerat { get; set; }
    public string? Ville { get; set; }

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? endDate { get; set; }
    public bool? GroupedByPharmacie { get; set; }
   
}
public class DateTimeConverter : JsonConverter<DateTime?>
{
       public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var str = reader.GetString();
            if (DateTime.TryParse(str, out var date))
                return date;
        }
        return null;
    }



    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToString("o"));
        else
            writer.WriteNullValue();
    }
}


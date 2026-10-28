using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmployeeApi.DTOs;

public class AttendanceDto
{
    [Range(1, int.MaxValue)]
    public int EmployeeId { get; set; }
    [JsonConverter(typeof(BlankDateOnlyUsesTodayConverter))]
    public DateOnly WorkDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public TimeOnly? CheckIn { get; set; }
    public TimeOnly? CheckOut { get; set; }
    [StringLength(300)]
    public string? Notes { get; set; }
}

public sealed class BlankDateOnlyUsesTodayConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var text = reader.GetString();
            if (string.IsNullOrWhiteSpace(text))
                return DateOnly.FromDateTime(DateTime.Today);

            if (DateOnly.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                return date;
        }

        throw new JsonException("WorkDate must be empty or a valid date such as 2026-10-20.");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
}

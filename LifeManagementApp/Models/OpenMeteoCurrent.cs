using System.Text.Json.Serialization;

namespace LifeManagementApp.Models;

// DTO: "current"-objekti JSONista. JsonPropertyName kertoo mikä JSON-kenttä menee mihin propertyyn
public class OpenMeteoCurrent
{
    [JsonPropertyName("time")]
    public string? Time { get; set; }              // esim. "2026-11-21T15:00" – parsitaan itse

    [JsonPropertyName("temperature_2m")]
    public double Temperature2m { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public double WindSpeed10m { get; set; }
}

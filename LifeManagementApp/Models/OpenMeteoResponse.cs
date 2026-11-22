using System.Text.Json.Serialization;

namespace LifeManagementApp.Models;

// DTO: vastaa Open-Meteon JSONin ylintä tasoa. Meitä kiinnostaa vaan "current"
public class OpenMeteoResponse
{
    [JsonPropertyName("current")]
    public OpenMeteoCurrent? Current { get; set; }
}

using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using LifeManagementApp.Models;

namespace LifeManagementApp.Services;

// Hakee sään Open-Meteosta (ei tarvii API-avainta)
public class OpenMeteoWeatherService : IWeatherService
{
    private readonly HttpClient _http;

    public OpenMeteoWeatherService(HttpClient http)
    {
        _http = http;
    }

    public async Task<WeatherInfo> GetCurrentWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
    {
        // InvariantCulture tärkee! Suomi-asetuksilla 63.10 muuttuis "63,10" ja URL hajois
        var lat = latitude.ToString(CultureInfo.InvariantCulture);
        var lon = longitude.ToString(CultureInfo.InvariantCulture);

        // wind_speed_unit=ms -> tuuli m/s eikä km/h. timezone=auto -> aika paikallisena
        var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}" +
                  "&current=temperature_2m,wind_speed_10m&wind_speed_unit=ms&timezone=auto";

        // GetFromJsonAsync: tekee GET-pyynnön ja muuttaa JSONin suoraan OpenMeteoResponse-olioks
        var response = await _http.GetFromJsonAsync<OpenMeteoResponse>(url, cancellationToken);

        if (response?.Current is null)
            throw new JsonException("Weather response did not contain current data.");

        // DTO -> oma malli
        return new WeatherInfo
        {
            TemperatureC = response.Current.Temperature2m,
            WindSpeedMs = response.Current.WindSpeed10m,
            Time = DateTime.TryParse(response.Current.Time, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time)
                ? time
                : DateTime.Now
        };
    }
}

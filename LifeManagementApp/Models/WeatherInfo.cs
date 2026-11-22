namespace LifeManagementApp.Models;

// Sovelluksen oma säämalli – UI ei tiiä mitään Open-Meteon JSON-muodosta
public class WeatherInfo
{
    public double TemperatureC { get; set; }
    public double WindSpeedMs { get; set; }   // Metriä sekunnissa (wind_speed_unit=ms)
    public DateTime Time { get; set; }        // Milloin mitattu
}

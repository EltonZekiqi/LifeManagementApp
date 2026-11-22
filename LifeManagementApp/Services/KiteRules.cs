namespace LifeManagementApp.Services;

// Leijasurffisääntö yhessä paikassa – helppo testata ja muuttaa
public static class KiteRules
{
    public const double DefaultWindLimitMs = 5.0;

    // "Yli 5 m/s" -> tasan 5.0 ei vielä riitä
    public static bool IsKiteWeather(double windSpeedMs, double limitMs = DefaultWindLimitMs)
    {
        return windSpeedMs > limitMs;
    }
}

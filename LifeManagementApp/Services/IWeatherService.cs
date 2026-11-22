using LifeManagementApp.Models;

namespace LifeManagementApp.Services;

// Sopimus säälle. Vois vaihtaa toiseen palveluun ilman et ViewModel muuttuu
public interface IWeatherService
{
    Task<WeatherInfo> GetCurrentWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
}

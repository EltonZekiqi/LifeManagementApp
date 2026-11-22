using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifeManagementApp.Services;

namespace LifeManagementApp.ViewModels;

// Etusivun logiikka. Ei tiedä mitään XAML-kontrolleista, vaan dataa ja komentoja
public partial class HomeViewModel : ObservableObject
{
    // Vaasa
    private const double Latitude = 63.10;
    private const double Longitude = 21.62;

    private readonly ITodoRepository _todoRepository;
    private readonly IWeatherService _weatherService;

    public HomeViewModel(ITodoRepository todoRepository, IWeatherService weatherService)
    {
        _todoRepository = todoRepository;
        _weatherService = weatherService;
    }

    // ObservableCollection kertoo UI:lle kun rivejä lisätään/poistetaan (List ei kerro)
    public ObservableCollection<TodoItem> TopTodos { get; } = new();

    // [ObservableProperty] generoi propertyn "Weather" + PropertyChanged-ilmotuksen
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasWeather))] // Kun Weather muuttuu, myös HasWeather päivittyy UI:ssa
    private WeatherInfo? weather;

    [ObservableProperty]
    private bool isKiteWeather;

    [ObservableProperty]
    private bool isBusy;            // true = ladataan, näytetään pyörivä indikaattori

    [ObservableProperty]
    private string? errorMessage;

    public bool HasWeather => Weather is not null;

    public double WindLimitMs => KiteRules.DefaultWindLimitMs;

    // [RelayCommand] generoi "LoadCommand"in (Async-pääte lähtee nimestä pois)
    [RelayCommand]
    public async Task LoadAsync()
    {
        await LoadTodosAsync();
        await LoadWeatherAsync();
    }

    private async Task LoadTodosAsync()
    {
        var all = await _todoRepository.GetAllAsync();

        TopTodos.Clear();
        foreach (var todo in TodoSorter.GetTop3(all))
            TopTodos.Add(todo);
    }

    private async Task LoadWeatherAsync()
    {
        IsBusy = true;
        ErrorMessage = null;

        try
        {
            // await -> UI ei jäädy sillä aikaa ku odotetaan nettii
            var result = await _weatherService.GetCurrentWeatherAsync(Latitude, Longitude);
            Weather = result;
            IsKiteWeather = KiteRules.IsKiteWeather(result.WindSpeedMs);
        }
        catch (HttpRequestException)
        {
            // Ei nettiä tai palvelin ei vastaa -> viesti eikä kaatumista
            ErrorMessage = "Could not load weather. Check your internet connection.";
        }
        catch (TaskCanceledException)
        {
            // HttpClientin timeout (10 s) laukes
            ErrorMessage = "Weather request timed out. Try again.";
        }
        catch (JsonException)
        {
            ErrorMessage = "Weather data was in the wrong format.";
        }
        finally
        {
            // finally ajetaan aina, onnistu tai ei
            IsBusy = false;
        }
    }
}

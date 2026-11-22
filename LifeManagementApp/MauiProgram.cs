using LifeManagementApp.Data;
using LifeManagementApp.Services;
using LifeManagementApp.ViewModels;
using LifeManagementApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

namespace LifeManagementApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ---------- Tietokanta ----------
        // SQLite-tiedosto sovelluksen omaan datakansioon
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "lifemanagement.db3");
        // Factory tekee uuden lyhytikäsen DbContextin aina ku repository tarvii
        builder.Services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlite($"Filename={dbPath}"));

        // ---------- Palvelut: Singleton ----------
        // Yks instanssi koko sovellukselle: ei omaa tilaa, joten sitä voi jakaa
        builder.Services.AddSingleton<ITodoRepository, TodoRepository>();
        // Yks HttpClient uudelleenkäyttöön (uus joka kerta -> socketit loppuu)
        builder.Services.AddSingleton(_ => new HttpClient { Timeout = TimeSpan.FromSeconds(10) });
        builder.Services.AddSingleton<IWeatherService, OpenMeteoWeatherService>();

        // ---------- ViewModelit ja sivut: Transient ----------
        // Uus instanssi joka kerta -> esim. lomake on aina tyhjä/puhdas kun se avataan
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<TodoListViewModel>();
        builder.Services.AddTransient<AddEditTodoViewModel>();

        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<TodoListPage>();
        builder.Services.AddTransient<AddEditTodoPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Code First: luodaan kanta ja taulut mallien perusteella jos niitä ei oo vielä
        using (var db = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext())
        {
            db.Database.EnsureCreated();
        }

        return app;
    }
}

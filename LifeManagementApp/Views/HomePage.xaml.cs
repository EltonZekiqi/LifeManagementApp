using LifeManagementApp.ViewModels;

namespace LifeManagementApp.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

    // DI antaa ViewModelin konstruktoriin
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    // Aina kun sivu tulee näkyviin -> päivitetään top 3 ja sää
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}

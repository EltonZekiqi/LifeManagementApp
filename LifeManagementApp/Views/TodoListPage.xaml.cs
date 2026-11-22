using LifeManagementApp.ViewModels;

namespace LifeManagementApp.Views;

public partial class TodoListPage : ContentPage
{
    private readonly TodoListViewModel _viewModel;

    public TodoListPage(TodoListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    // Kun tullaan takas lomakkeelta, lista ladataan uudestaan kannasta
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}

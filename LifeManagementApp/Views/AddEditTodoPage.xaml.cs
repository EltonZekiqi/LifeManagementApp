using LifeManagementApp.ViewModels;

namespace LifeManagementApp.Views;

public partial class AddEditTodoPage : ContentPage
{
    private readonly AddEditTodoViewModel _viewModel;

    public AddEditTodoPage(AddEditTodoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    // Parametrit (TodoId) on jo tullu ApplyQueryAttributesin kautta -> ladataan data
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}

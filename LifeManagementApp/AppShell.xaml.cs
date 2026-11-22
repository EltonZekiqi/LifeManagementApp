using LifeManagementApp.Services;
using LifeManagementApp.Views;

namespace LifeManagementApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Lomakesivu ei oo välilehti, joten se rekisteröidään reitiks erikseen
        Routing.RegisterRoute(AppRoutes.AddEditTodo, typeof(AddEditTodoPage));
    }
}

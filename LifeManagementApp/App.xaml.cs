namespace LifeManagementApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    // .NET 9 tapa: ikkuna jonka sisällä on AppShell (navigointi)
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}

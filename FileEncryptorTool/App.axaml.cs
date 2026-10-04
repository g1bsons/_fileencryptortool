using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace FileEncryptorTool;

public partial class App : Application
{
    // This is the loading of the AXAML files to generate the look and layout of the GUI
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // This is the method that creates the window for the user to interact with
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
using System;
using Avalonia;

namespace FileEncryptorTool;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    // This is the starting point in the program that creates the GUI 
    // and creates the avalonia app
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // This method configures the avalonia app
    // UsePlatformDetect allows avalonia to determine whether the
    // programs OS between linux, windows, or some other OS
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();
}

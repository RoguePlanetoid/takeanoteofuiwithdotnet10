namespace MauiNotes;

/// <summary>
/// Maui Program
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Create Maui App
    /// </summary>
    /// <returns>Maui App</returns>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .RegisterServices()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Calligraffitti-Regular.ttf", "Calligraffitti");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

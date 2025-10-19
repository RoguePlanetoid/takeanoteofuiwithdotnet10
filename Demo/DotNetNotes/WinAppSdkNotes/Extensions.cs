namespace WinAppSdkNotes;

/// <summary>
/// Extensions
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Register Services
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <returns>Service Collection</returns>
    public static IServiceCollection RegisterServices(
        this IServiceCollection services) => 
        services.AddServices()
        .AddSingleton<FileProvider>()
        .AddSingleton<WidgetService>()
        .AddTransient<WinAppSdkDialog>()
        .AddTransient<MainWindow>();
}

namespace UwpNotes;

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
    [RequiresUnreferencedCode("Calls DotNetNotes.Extensions.AddServices(NotesConfig)")]
    [RequiresDynamicCode("Calls DotNetNotes.Extensions.AddServices(NotesConfig)")]
    public static IServiceCollection RegisterServices(
        this IServiceCollection services) => 
        services.AddServices()
        .AddTransient<UwpDialog>()
        .AddTransient<MainPage>();
}

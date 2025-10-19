namespace ConsoleNotes;

/// <summary>
/// Extensions
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Register Services
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="args">Command Line Arguments</param>
    /// <returns>Service Collection</returns>
    public static IServiceCollection RegisterServices(
        this IServiceCollection service, string[] args) =>
        service.AddSingleton(new ConsoleArgumentProvider(args))
        .AddSingleton<ConsoleProvider>()
        .AddServices();
}

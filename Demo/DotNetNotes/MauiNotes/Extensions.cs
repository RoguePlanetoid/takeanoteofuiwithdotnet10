namespace MauiNotes;

/// <summary>
/// Extensions
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Register Services
    /// </summary>
    /// <param name="builder">MauiAppBuilder</param>
    /// <returns>MauiAppBuilder</returns>
    public static MauiAppBuilder RegisterServices(
        this MauiAppBuilder builder)
    {
        builder.Services.AddServices(new NotesConfig()
        {
             ConnectionString = $"Filename={FileSystem.AppDataDirectory}/notes.db"
        })
        .AddTransient<MainPage>()
        .AddTransient<MauiDialog>();
        return builder;
    }
}

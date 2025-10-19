namespace WinFormsNotes;

/// <summary>
/// Program
/// </summary>
internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
                services.RegisterServices())
            .Build();
        _ = host.Services.GetRequiredService<TrayNotes>();
        var form = host.Services.GetRequiredService<MainForm>();
        Application.Run(form);
    }
}
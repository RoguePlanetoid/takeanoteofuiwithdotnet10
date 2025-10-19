// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinAppSdkNotes;

/// <summary>
/// Provides application-specific behaviour to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private const string headless = "-headless";

    private static WidgetService? _service;

    /// <summary>
    /// Host
    /// </summary>
    public static IHost? Host { get; private set; }

    /// <summary>
    /// Activate Main Window
    /// </summary>
    private static void ActivateMainWindow() =>
        Host?.Services.GetRequiredService<MainWindow>()?.Activate();

    /// <summary>
    /// Start Service Host
    /// </summary>
    private static async Task StartServiceHost()
    {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
        .ConfigureServices(services => services.RegisterServices())
        .Build();
        await Host!.StartAsync();
        _service = Host.Services.GetRequiredService<WidgetService>();
    }

    /// <summary>
    /// Register Widget Provider
    /// </summary>
    private static void RegisterWidgetProvider()
    {
        ComWrappersSupport.InitializeComWrappers();
        WidgetProvider.AddWidget(WidgetNotes.DefinitionId,
        (widgetId, initialState) =>
        new WidgetNotes(_service!, widgetId, initialState));
        WidgetRegistrationManager<WidgetProvider>.RegisterProvider();
    }

    /// <summary>
    /// Is Headless?
    /// </summary>
    /// <returns>True if Is, False if Not</returns>
    private static bool IsHeadless() =>
        Environment.GetCommandLineArgs().Contains(headless);

    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App() =>
        InitializeComponent();

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        await StartServiceHost();
        RegisterWidgetProvider();
        if (!IsHeadless())
            ActivateMainWindow();
        base.OnLaunched(args);
    }
}

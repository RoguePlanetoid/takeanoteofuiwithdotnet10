// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinAppSdkNotes;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="application">Application</param>
    /// <param name="dialog">Dialog</param>
    public MainWindow(IApplicationProvider application, WinAppSdkDialog dialog)
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        Display.DataContext = application.Content;
        application.Confirm = async (DialogModel model) =>
            await dialog.DeleteAsync(this, model);
        application.Upsert = async (DialogModel model) =>
            await dialog.UpsertAsync(this, model);
    }
}
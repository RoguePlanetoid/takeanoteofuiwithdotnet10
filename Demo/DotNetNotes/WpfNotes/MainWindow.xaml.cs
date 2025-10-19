namespace WpfNotes;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="application">Application</param>
    /// <param name="dialog">Dialog</param>
    public MainWindow(IApplicationProvider application, WpfDialog dialog)
    {
        InitializeComponent();
        Display.DataContext = application.Content;
        application.Confirm = dialog.DeleteAsync;
        application.Upsert = dialog.UpsertAsync;
    }
}
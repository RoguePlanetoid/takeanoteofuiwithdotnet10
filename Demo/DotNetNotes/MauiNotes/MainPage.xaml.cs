namespace MauiNotes;

/// <summary>
/// Interaction Logic for MainPage.xaml
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="application">Application</param>
    /// <param name="dialog">Dialog</param>
    public MainPage(IApplicationProvider application, MauiDialog dialog)
    {
        InitializeComponent();
        BindingContext = application.Content;
        application.Confirm = async (DialogModel model) =>
            await MauiDialog.DeleteAsync(this, model);
        application.Upsert = async (DialogModel model) =>
            await dialog.UpsertAsync(this, model);
    }
}

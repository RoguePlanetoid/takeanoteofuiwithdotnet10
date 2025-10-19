namespace WinAppSdkNotes.Controls;

/// <summary>
/// Windows App SDK Dialog
/// </summary>
/// <param name="application">Application</param>
public class WinAppSdkDialog(IApplicationProvider application)
{
    private const string upsert_template = "UpsertTemplate";
    private const string delete_template = "DeleteTemplate";
    private ContentDialog? _dialog = null;

    /// <summary>
    /// Confirm
    /// </summary>
    /// <param name="xamlRoot">Root</param>
    /// <param name="title">Title</param>
    /// <param name="content">Content</param>
    /// <param name="primaryButtonText">Primary Button Text</param>
    /// <param name="secondaryButtonText">Secondary Button Text</param>
    /// <returns>True if Primary Button Selected False if Not</returns>
    private async Task<bool> ConfirmAsync(XamlRoot xamlRoot, string title, object content,
        string primaryButtonText = "Ok", string secondaryButtonText = "")
    {
        try
        {
            _dialog?.Hide();
            _dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                XamlRoot = xamlRoot,
                PrimaryButtonText = primaryButtonText,
                SecondaryButtonText = secondaryButtonText
            };
            _dialog.PrimaryButtonClick += (sender, args) =>
            {
                var model = (content as ContentControl)?.Content as DialogModel;
                args.Cancel = model?.IsValid == false;
            };
            return await _dialog.ShowAsync() == ContentDialogResult.Primary;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="mainWindow">Main Window</param>
    /// <param name="content">Dialog Content</param>
    /// <returns>True if Primary Button Selected False if Not</returns>
    public async Task<bool> DeleteAsync(MainWindow mainWindow, DialogModel content) =>
        await ConfirmAsync(mainWindow.Content.XamlRoot, content.Title, new ContentControl()
        {
            Content = content,
            ContentTemplate = Application.Current.Resources[delete_template] as DataTemplate
        }, content.PrimaryOption, content.SecondaryOption);

    /// <summary>
    /// Upsert
    /// </summary>
    /// <param name="mainWindow">Main Window</param>
    /// <param name="content">Dialog Content</param>
    /// <returns>Note if Primary Button Selected, Null if Not</returns>
    public async Task<bool> UpsertAsync(MainWindow mainWindow, DialogModel content)
    {
        var result = await ConfirmAsync(mainWindow.Content.XamlRoot, content.Title, new ContentControl()
        {
            Content = content,
            ContentTemplate = Application.Current.Resources[upsert_template] as DataTemplate
        }, content.PrimaryOption, content.SecondaryOption);
        if (result)
        {
            application.Content.Note = content.Note;
            return true;
        }
        return false;
    }
}

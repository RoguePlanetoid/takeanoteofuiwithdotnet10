namespace UwpNotes.Controls;

/// <summary>
/// Universal Windows Platform Dialog
/// </summary>
/// <param name="application">Application</param>
public class UwpDialog(IApplicationProvider application)
{
    /// <summary>
    /// Confirm
    /// </summary>
    /// <param name="xamlRoot">Root</param>
    /// <param name="title">Title</param>
    /// <param name="content">Content</param>
    /// <param name="primaryButtonText">Primary Button Text</param>
    /// <param name="secondaryButtonText">Secondary Button Text</param>
    /// <returns>True if Primary Button Selected False if Not</returns>
    private static async Task<bool> ConfirmAsync(ContentDialog dialog)
    {
        try
        {
            dialog.Hide();
            dialog.PrimaryButtonClick += (sender, args) =>
            {
                var model = sender.Tag as DialogModel;
                args.Cancel = model?.IsValid == false;
            };
            return await dialog.ShowAsync() == ContentDialogResult.Primary;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="dialog">Content Dialog</param>    
    /// <returns>True if Primary Button Selected False if Not</returns>
    public static async Task<bool> DeleteAsync(ContentDialog dialog) =>
        await ConfirmAsync(dialog);

    /// <summary>
    /// Upsert
    /// </summary>
    /// <param name="dialog">Content Dialog</param>
    /// <param name="model">Dialog Model</param>
    /// <returns>Note if Primary Button Selected, Null if Not</returns>
    public async Task<bool> UpsertAsync(ContentDialog dialog, DialogModel? model)
    {
        var result = await ConfirmAsync(dialog);
        if (result)
        {
            application.Content.Note = model!.Note;
            return true;
        }
        return false;
    }
}

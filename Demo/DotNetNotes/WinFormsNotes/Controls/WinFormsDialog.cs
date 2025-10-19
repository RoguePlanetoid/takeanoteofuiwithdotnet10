namespace WinFormsNotes.Controls;

/// <summary>
/// WinForms Dialog
/// </summary>
/// <param name="application">Application</param>
public class WinFormsDialog(IApplicationProvider application)
{
    private const string delete_message = "Are you sure you want to delete this note?";

    /// <summary>
    /// Confirm
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="message">Content</param>
    /// <returns>True if Primary Button Selected, False if Not</returns>
    private static bool Confirm(string title, string message) => 
        MessageBox.Show(message, title, 
            MessageBoxButtons.YesNo, 
            MessageBoxIcon.Question) == DialogResult.Yes;

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="content">Dialog Content</param>
    /// <returns>True if Primary Button Selected, False if Not</returns>
    public static Task<bool> DeleteAsync(DialogModel content) => 
        Task.FromResult(Confirm(content.Title, delete_message));

    /// <summary>
    /// Upsert
    /// </summary>
    /// <param name="mainForm">Main Form</param>
    /// <param name="content">Dialog Content</param>
    /// <returns>True if Primary Button Selected, False if Not</returns>
    public async Task<bool> UpsertAsync(Form mainForm, DialogModel content)
    {
        using var dialog = new UpsertDialog(content);
        // Async Forms
        var result = await dialog.ShowDialogAsync(mainForm) == DialogResult.OK;
        if(result)
            application.Content.Note = content.Note;
        return result;
    }
}

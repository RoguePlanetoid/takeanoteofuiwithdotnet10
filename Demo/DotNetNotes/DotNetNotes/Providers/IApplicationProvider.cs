namespace DotNetNotes.Providers;

/// <summary>
/// Application Provider
/// </summary>
public interface IApplicationProvider
{
    /// <summary>
    /// New
    /// </summary>
    /// <param name="model">Note Model</param>
    /// <param name="skipDialog">Skip Dialog</param>
    /// <returns>True on Success, otherwise False</returns>
    [RequiresUnreferencedCode("Calls DotNetNotes.Provider.ApplicationProvider.NewAsync() which calls IsValid(NoteModel, List<ValidationResult>).")]
    Task<bool> NewAsync(NoteModel? model = null, bool skipDialog = false);

    /// <summary>
    /// Edit
    /// </summary>
    /// <param name="model">Note Model</param>
    /// <param name="skipDialog">Skip Dialog</param>
    /// <returns>True on Success, otherwise False</returns>
    [RequiresUnreferencedCode("Calls DotNetNotes.Provider.ApplicationProvider.IsValid(NoteModel, List<ValidationResult>)")]
    Task<bool> EditAsync(NoteModel? model, bool skipDialog = false);

    /// <summary>
    /// Delete Note
    /// </summary>
    /// <param name="note">Note Model</param>
    /// <param name="skipDialog">Skip Dialog</param>
    /// <returns>True on Success, otherwise False</returns>
    Task<bool> DeleteAsync(NoteModel? note, bool skipDialog = false);

    /// <summary>
    /// Get Note
    /// </summary>
    /// <param name="id">Note Id</param>
    /// <returns>Note Model</returns>
    Task<NoteModel?> GetAsync(int? id);

    /// <summary>
    /// List
    /// </summary>
    /// <returns>List of Notes</returns>
    Task<ObservableCollection<NoteModel>> ListAsync();

    /// <summary>
    /// Confirm
    /// </summary>
    Func<DialogModel, Task<bool>> Confirm { get; set; }

    /// <summary>
    /// Upsert
    /// </summary>
    Func<DialogModel, Task<bool>> Upsert { get; set; }

    /// <summary>
    /// Updated
    /// </summary>
    Action Updated { get; set; }

    /// <summary>
    /// Content
    /// </summary>
    ContentModel Content { get; }
}

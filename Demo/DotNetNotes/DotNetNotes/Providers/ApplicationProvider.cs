namespace DotNetNotes.Providers;

/// <summary>
/// Application Provider
/// </summary>
internal class ApplicationProvider : IApplicationProvider
{
    private const string delete_note_title = "Delete Note?";
    private const string edit_note_title = "Edit Note";
    private const string new_note_title = "New Note";
    private const string cancel = "Cancel";
    private const string save = "Save";
    private const string yes = "Yes";
    private const string no = "No";

    private readonly INotesProvider _notes;

    /// <summary>
    /// Get Confirm
    /// </summary>
    /// <param name="title"></param>
    /// <param name="note">note</param>
    /// <returns>Dialog Model</returns>
    private static DialogModel GetConfirm(string title, NoteModel note) => new()
    {
        Title = title,
        Note = note,
        PrimaryOption = yes,
        SecondaryOption = no
    };

    /// <summary>
    /// Get Upsert
    /// </summary>
    /// <param name="title"></param>
    /// <param name="note">Note</param>
    /// <returns>Dialog Model</returns>
    private static DialogModel GetUpsert(string title, NoteModel note) => new()
    {
        Title = title,
        Note = note,
        PrimaryOption = save,
        SecondaryOption = cancel
    };

    /// <summary>
    /// Is Valid
    /// </summary>
    /// <param name="model">Model</param>
    /// <param name="results">Results</param>
    /// <returns>True if Valid, False if Not</returns>
    [RequiresUnreferencedCode("Calls System.ComponentModel.DataAnnotations.Validator.TryValidateObject(Object, ValidationContext, ICollection<ValidationResult>, Boolean)")]
    private static bool IsValid(NoteModel model, List<ValidationResult> results) =>
        Validator.TryValidateObject(model, new ValidationContext(model), results, true);

    /// <summary>
    /// List
    /// </summary>
    private async void List() =>
        Content.Notes = new ObservableCollection<NoteModel>(await _notes.ListAsync());

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="notes">Notes</param>
    [RequiresUnreferencedCode("Calls DotNetNotes.Provider.ApplicationProvider.NewAsync() and EditAsync(NoteModel?) which call IsValid(NoteModel, List<ValidationResult>).")]
    public ApplicationProvider(INotesProvider notes)
    {
        _notes = notes;
        Content = new ContentModel(
            (p) => List(),
            async (p) => await NewAsync(),
            async (p) => await EditAsync(p as NoteModel),
            async (p) => await DeleteAsync(p as NoteModel)
        );
        List();
    }

    /// <summary>
    /// New
    /// </summary>
    /// <param name="model">Note Model</param>
    /// <param name="skipDialog">Skip Dialog</param>
    /// <returns>True on Success, otherwise False</returns>
    [RequiresUnreferencedCode("Calls DotNetNotes.Provider.ApplicationProvider.IsValid(NoteModel, List<ValidationResult>)")]
    public async Task<bool> NewAsync(NoteModel? model = null, bool skipDialog = false)
    {
        Content.Note = model ?? new();
        var dialog = skipDialog || await Upsert(GetUpsert(new_note_title, Content.Note));
        if (dialog && IsValid(Content.Note, []) && await _notes.CreateAsync())
        {
            var id = await _notes.AddAsync(Content.Note);
            if (id != null)
            {
                Content.Note.Id = id;
                Content.Notes.Add(Content.Note);
                Updated();
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Edit
    /// </summary>
    /// <param name="model">Note Model</param>
    /// <param name="skipDialog">Skip Dialog</param>
    /// <returns>True on Success, otherwise False</returns>
    [RequiresUnreferencedCode("Calls DotNetNotes.Provider.ApplicationProvider.IsValid(NoteModel, List<ValidationResult>)")]
    public async Task<bool> EditAsync(NoteModel? model, bool skipDialog = false)
    {
        if (model != null)
        {
            Content.Note = model;
            var title = Content.Note.Title;
            var content = Content.Note.Content;
            var background = Content.Note.Background;
            var dialog = skipDialog || await Upsert(GetUpsert(edit_note_title, Content.Note));
            if (!dialog || !IsValid(Content.Note, []) || Content.Note.Id == null ||
                !await _notes.EditAsync(Content.Note.Id.Value, Content.Note))
                    (Content.Note.Title, Content.Note.Content, Content.Note.Background) =
                        (title, content, background);
            else
            {
                await ListAsync();
                Updated();
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Delete Note
    /// </summary>
    /// <param name="note">Note</param>
    /// <param name="skipDialog">Skip Dialog</param>
    /// <returns>True on Success, otherwise False</returns>
    public async Task<bool> DeleteAsync(NoteModel? note, bool skipDialog = false)
    {
        var remove = Content.Notes.First(n => n.Id == note?.Id);
        var result = note?.Id != null && (skipDialog || await Confirm(GetConfirm(delete_note_title, note))) &&
            await _notes.DeleteAsync(note.Id.Value) && remove != null && Content.Notes.Remove(remove);
        if (result)
            Updated();
        return result;
    }

    /// <summary>
    /// Get Note
    /// </summary>
    /// <param name="id">Note Id</param>
    /// <returns>Note Model</returns>
    public async Task<NoteModel?> GetAsync(int? id) =>
        id != null ? await _notes.GetAsync(id.Value) : null;

    /// <summary>
    /// List
    /// </summary>
    /// <returns>List of Notes</returns>
    public async Task<ObservableCollection<NoteModel>> ListAsync()
    {
        Content.Notes.Clear();
        foreach (var note in await _notes.ListAsync())
        {
            if (Content.Notes.Select(n => n.Id).Contains(note.Id))
                continue;
            Content.Notes.Add(note);
        }
        return Content.Notes;
    }

    /// <summary>
    /// Confirm
    /// </summary>
    public Func<DialogModel, Task<bool>> Confirm { get; set; } =
        (confirm) => Task.FromResult(false);

    /// <summary>
    /// Upsert
    /// </summary>
    public Func<DialogModel, Task<bool>> Upsert { get; set; } =
        (confirm) => Task.FromResult(false);

    /// <summary>
    /// Updated
    /// </summary>
    public Action Updated { get; set; } = () => { };

    /// <summary>
    /// Content
    /// </summary>
    public ContentModel Content { get; private set; }
}

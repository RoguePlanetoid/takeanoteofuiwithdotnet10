namespace AspNetCoreWebAppNotes.Pages;

/// <summary>
/// Edit Model
/// </summary>
/// <param name="application">Application Provider</param>
public class EditModel(IApplicationProvider application) : PageModel
{
    /// <summary>
    /// Note Id
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public int NoteId { get; set; } = new();

    /// <summary>
    /// Note
    /// </summary>
    [BindProperty]
    public NoteModel Note { get; set; } = new();

    /// <summary>
    /// Get
    /// </summary>
    /// <param name="noteId">Note Id</param>
    /// <returns></returns>
    public async Task OnGetAsync(int noteId)
    {
        var note = await application.GetAsync(noteId);
        if (note != null)
            Note = note;
    }

    /// <summary>
    /// Edit
    /// </summary>
    public async Task OnPostEditAsync()
    {
        if (ModelState.IsValid)
        {
            await application.EditAsync(Note, true);
            Response.Redirect("/");
        }
    }

    /// <summary>
    /// Delete
    /// </summary>
    public async Task OnPostDeleteAsync()
    {
        var note = await application.GetAsync(NoteId);
        if (note != null)
            await application.DeleteAsync(note, true);
        Response.Redirect("/");
    }
}

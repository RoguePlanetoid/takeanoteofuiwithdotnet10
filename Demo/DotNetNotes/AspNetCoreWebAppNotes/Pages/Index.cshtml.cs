namespace AspNetCoreWebAppNotes.Pages;

/// <summary>
/// Index Model
/// </summary>
/// <param name="application">Application Provider</param>
public class IndexModel(IApplicationProvider application) : PageModel
{
    /// <summary>
    /// Note
    /// </summary>
    [BindProperty]
    public NoteModel Note { get; set; } = new();

    /// <summary>
    /// Notes
    /// </summary>
    public IEnumerable<NoteModel> Notes { get; set; } = [];

    /// <summary>
    /// Get
    /// </summary>    
    public async Task OnGetAsync() =>
        Notes = await application.ListAsync();

    /// <summary>
    /// Submit
    /// </summary>
    public async Task OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            await application.NewAsync(Note, true);
        }
        Notes = await application.ListAsync();
    }
}

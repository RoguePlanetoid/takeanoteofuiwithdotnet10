namespace McpNotes.Tools;

/// <summary>
/// Notes Tools
/// </summary>
/// <param name="application">Application</param>
internal class NotesTools(IApplicationProvider application)
{
    private const char hash = '#';
    private const string unknown = "Unknown";   

    /// <summary>
    /// List Notes
    /// </summary>
    /// <returns>List of Notes</returns>
    [McpServerTool]
    [Description("Get a list of all notes with their details")]
    public async Task<object> ListNotes()
    {
        var notes = await application.ListAsync();
        return notes.Select(note => new
        {
            id = note.Id,            
            title = note.Title,
            content = note.Content,
            background = note.Background,
            backgroundName = BackgroundModel.BackgroundToName?.GetValueOrDefault(note.Background, unknown)
        }).ToList();
    }

    /// <summary>
    /// Get Note
    /// </summary>
    /// <param name="id">Note Id</param>
    /// <returns>Note</returns>
    /// <exception cref="InvalidOperationException">Note with ID not found</exception>
    [McpServerTool]
    [Description("Get a specific note by its ID")]
    public async Task<object> GetNote([Description("The ID of the note to retrieve")] int id)
    {
        var note = await application.GetAsync(id);
        return note == null ? throw new InvalidOperationException($"Note with ID {id} not found")
        : (object)(new
        {
            id = note.Id,
            title = note.Title,
            content = note.Content,
            background = note.Background,
            backgroundName = BackgroundModel.BackgroundToName?.GetValueOrDefault(note.Background, unknown)
        });
    }

    /// <summary>
    /// Create Note
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="content">Content</param>
    /// <param name="background">Background</param>
    /// <returns>Note</returns>
    /// <exception cref="ArgumentException">Invalid Background</exception>
    /// <exception cref="InvalidOperationException">Invalid Title or Content</exception>
    [McpServerTool]
    [Description("Create a new note")]
    public async Task<object> CreateNote(
        [Description("The title of the note (max 50 characters)")] string title,
        [Description("The content of the note (max 255 characters)")] string content,
        [Description("The background colour (red, orange, yellow, green, blue, indigo, violet)")] string? background = null)
    {
        var note = new NoteModel
        {
            Title = title,
            Content = content
        };
        if (!string.IsNullOrEmpty(background))
        {
            if (BackgroundModel.NameToBackground?.ContainsKey(background.ToLower()) == true)
                note.Background = BackgroundModel.NameToBackground[background.ToLower()];
            else if (background.StartsWith(hash) && BackgroundModel.Values.Contains(background))
                note.Background = background;
            else
                throw new ArgumentException($"Invalid background colour. Use one of: {BackgroundModel.NamesOutput}");
        }
        var success = await application.NewAsync(note, true);
        if (!success)
            throw new InvalidOperationException("Failed to create note. Please check the title (max 50 chars) and content (max 255 chars).");
        return new
        {
            message = "Note created successfully",
            note = new
            {
                id = note.Id,
                title = note.Title,
                content = note.Content,
                background = note.Background,
                backgroundName = BackgroundModel.BackgroundToName?.GetValueOrDefault(note.Background, unknown)
            }
        };
    }

    /// <summary>
    /// Update Note
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="title">Title</param>
    /// <param name="content">Content</param>
    /// <param name="background">Background</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Note with Id not Found</exception>
    /// <exception cref="ArgumentException">Invalid Background</exception>
    [McpServerTool]
    [Description("Update an existing note")]
    public async Task<object> UpdateNote(
        [Description("The ID of the note to update")] int id,
        [Description("The new title (optional)")] string? title = null,
        [Description("The new content (optional)")] string? content = null,
        [Description("The new background colour (optional)")] string? background = null)
    {
        var note = await application.GetAsync(id) ?? throw new InvalidOperationException($"Note with ID {id} not found");
        if (!string.IsNullOrEmpty(title))
            note.Title = title;
        if (!string.IsNullOrEmpty(content))
            note.Content = content;
        if (!string.IsNullOrEmpty(background))
        {
            if (BackgroundModel.NameToBackground?.ContainsKey(background.ToLower()) == true)
                note.Background = BackgroundModel.NameToBackground[background.ToLower()];
            else if (background.StartsWith(hash) && BackgroundModel.Values.Contains(background))
                note.Background = background;
            else
                throw new ArgumentException($"Invalid background colour. Use one of: {BackgroundModel.NamesOutput}");
        }
        var success = await application.EditAsync(note, true);
        if (!success)
            throw new InvalidOperationException("Failed to update note. Please check the title (max 50 chars) and content (max 255 chars).");
        return new
        {
            message = "Note updated successfully",
            note = new
            {
                id = note.Id,
                title = note.Title,
                content = note.Content,
                background = note.Background,
                backgroundName = BackgroundModel.BackgroundToName?.GetValueOrDefault(note.Background, unknown)
            }
        };
    }

    /// <summary>
    /// Delete Note
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Result</returns>
    /// <exception cref="InvalidOperationException">Note with Id not Found</exception>
    [McpServerTool]
    [Description("Delete a note by its ID")]
    public async Task<object> DeleteNote(
        [Description("The ID of the note to delete")] int id)
    {
        var note = await application.GetAsync(id) ?? throw new InvalidOperationException($"Note with ID {id} not found");         
        var success = await application.DeleteAsync(note, skipDialog: true);
        if (!success)
            throw new InvalidOperationException("Failed to delete note");
        return new
        {
            message = $"Note '{note.Title}' deleted successfully",
            deletedId = id
        };
    }

    /// <summary>
    /// Background Colours
    /// </summary>
    /// <returns></returns>
    [McpServerTool]
    [Description("Get the list of available background colors for notes")]
    public static object GetBackgroundColors() => 
        BackgroundModel.Options.Select(o => new
        {
            name = o.Name,
            hex = o.Background
        }).ToList();
}
namespace AspNetCoreWebApiNotes.Requests;

/// <summary>
/// Note Request
/// </summary>
public class NoteRequest
{
    /// <summary>
    /// Resolve Background
    /// </summary>
    /// <param name="input">Input</param>
    /// <returns>Background Name</returns>
    private static string? ResolveBackground(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;
        if (BackgroundModel.NameToBackground?.TryGetValue(input, out var fromName) == true)
            return fromName;
        if (BackgroundModel.BackgroundToName?.ContainsKey(input) == true)
            return input;
        return null;
    }

    [Required]
    [StringLength(50)]
    public string? Title { get; set; }

    [Required]
    [StringLength(255)]
    public string? Content { get; set; }

    [Required]
    [StringLength(10)]
    [AllowedBackground]
    public string? Background { get; set; }

    /// <summary>
    /// To Note Model
    /// </summary>
    /// <param name="request">Note Request</param>
    /// <returns>Note Model</returns>
    internal NoteModel ToNoteModel() =>
    new()
    {
        Title = Title ?? string.Empty,
        Content = Content ?? string.Empty,
        Background = ResolveBackground(Background) ?? string.Empty
    };
}

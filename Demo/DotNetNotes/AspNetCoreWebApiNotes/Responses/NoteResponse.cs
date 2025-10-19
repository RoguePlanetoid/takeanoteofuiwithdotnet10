namespace AspNetCoreWebApiNotes.Responses;

/// <summary>
/// Note Response
/// </summary>
public class NoteResponse
{
    /// <summary>
    /// Output Background
    /// </summary>
    /// <param name="background">Background</param>
    /// <returns>Background Name</returns>
    private static string? OutputBackground(string? background) =>
        string.IsNullOrWhiteSpace(background) ? null :
        BackgroundModel.BackgroundToName?.TryGetValue(background, out var name) == true ?
        name : background;

    /// <summary>
    /// Constuctor
    /// </summary>
    public NoteResponse() { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="model">Note Model</param>
    internal NoteResponse(NoteModel model)
    {
        Id = model.Id;
        Title = model.Title;
        Content = model.Content;
        Background = OutputBackground(model.Background);
        ImageUri = model.AssetDataUri;
    }

    /// <summary>
    /// Id
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Content
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Background
    /// </summary>
    public string? Background { get; set; }

    /// <summary>
    /// Image Uri
    /// </summary>
    public string? ImageUri { get; set; }
}

namespace DotNetNotes.Models;

/// <summary>
/// Dialog Model
/// </summary>
public class DialogModel
{
    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Note
    /// </summary>
    public NoteModel Note { get; set; } = new();

    /// <summary>
    /// Primary Option
    /// </summary>
    public string PrimaryOption { get; set; } = string.Empty;

    /// <summary>
    /// Secondary Option
    /// </summary>
    public string SecondaryOption { get; set; } = string.Empty;

    /// <summary>
    /// Colours
    /// </summary>
    public List<string> Colours => BackgroundModel.Values;

    /// <summary>
    /// Is Valid?
    /// </summary>
    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Note?.Title) &&
        !string.IsNullOrWhiteSpace(Note?.Content) &&
        !string.IsNullOrWhiteSpace(Note?.Background);
}

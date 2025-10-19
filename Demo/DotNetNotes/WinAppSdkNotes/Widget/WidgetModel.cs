namespace WinAppSdkNotes.Widget;

/// <summary>
/// Widget Model
/// </summary>
public class WidgetModel
{
    /// <summary>
    /// Image Id
    /// </summary>
    public int? ImageId { get; set; }

    /// <summary>
    /// Image
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Show Prev?
    /// </summary>
    public bool ShowPrev => Page > 1 && TotalPages > 1;

    /// <summary>
    /// Show Next?
    /// </summary>
    public bool ShowNext => Page < TotalPages && TotalPages > 1;

    /// <summary>
    /// Page
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Total Pages
    /// </summary>
    public int TotalPages { get; set; } = 1;

    /// <summary>
    /// Total Notes
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Note
    /// </summary>
    public NoteModel? Note { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    public List<NoteModel>? Notes { get; set; }

    /// <summary>
    /// Is Image Empty?
    /// </summary>
    public bool IsImageEmpty => 
        string.IsNullOrWhiteSpace(Image);
}

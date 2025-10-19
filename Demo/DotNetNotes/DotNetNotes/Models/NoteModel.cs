namespace DotNetNotes.Models;

/// <summary>
/// Note Model
/// </summary>
public class NoteModel : ObservableBase
{
    /// <summary>
    /// Id
    /// </summary>
    public int? Id 
    { 
        get => field; 
        set => SetProperty(ref field, value); 
    }

    /// <summary>
    /// Identifier
    /// </summary>
    public string? Identifier
    {
        get =>  Id?.ToString();
        set => Id = int.TryParse(value, out var id) ? id : null;
    }

    /// <summary>
    /// Title
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Title 
    { 
        get => field; 
        set => SetProperty(ref field, value); 
    } = string.Empty;

    /// <summary>
    /// Content
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Content 
    { 
        get => field; 
        set => SetProperty(ref field, value); 
    } = string.Empty;

    /// <summary>
    /// Background
    /// </summary>
    [Required]
    [StringLength(10)]
    public string Background
    {
        get => field;
        set => SetProperty(ref field, value);
    } = "#ffc476";
}

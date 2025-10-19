namespace UwpNotes.Binder;

/// <summary>
/// Note View Model
/// </summary>
[GeneratedBindableCustomProperty]
public partial class NoteViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Constructor
    /// </summary>
    public NoteViewModel() { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="model">Note Model</param>
    public NoteViewModel(NoteModel model) =>
        (Id, Title, Content, Background) = (model.Id, model.Title, model.Content, model.Background);

    /// <summary>
    /// Property Changed
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Id
    /// </summary>
    public int? Id
    {
        get => field;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
        }
    }

    /// <summary>
    /// Title
    /// </summary>
    public string Title
    {
        get => field;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
        }
    } = string.Empty;

    /// <summary>
    /// Content
    /// </summary>
    public string Content
    {
        get => field;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Content)));
        }
    } = string.Empty;

    /// <summary>
    /// Background
    /// </summary>
    public string Background
    {
        get => field;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Background)));
        }
    } = "#ffc476";

    /// <summary>
    /// As Note Model
    /// </summary>
    /// <param name="viewModel">Note View Model</param>
    public static NoteModel AsNoteModel(NoteViewModel viewModel) => new()
    {
        Id = viewModel.Id,
        Title = viewModel.Title,
        Content = viewModel.Content,
        Background = viewModel.Background
    };
}

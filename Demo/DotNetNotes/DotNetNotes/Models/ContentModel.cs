namespace DotNetNotes.Models;

/// <summary>
/// Content Model
/// </summary>
public class ContentModel : ObservableBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="listAction">List Action</param>
    /// <param name="newAction">New Action</param>
    /// <param name="editAction">Edit Action</param>
    /// <param name="deleteAction">Delete Action</param>
    public ContentModel(Action<object?> listAction, Action<object?> newAction,
        Action<object?> editAction, Action<object?> deleteAction) =>
        (ListAction, NewAction, EditAction, DeleteAction) =
            (new ActionCommand(listAction), new ActionCommand(newAction),
            new ActionCommand(editAction), new ActionCommand(deleteAction));

    /// <summary>
    /// List Action
    /// </summary>
    public ActionCommand ListAction { get; }

    /// <summary>
    /// New Action
    /// </summary>
    public ActionCommand NewAction { get; }

    /// <summary>
    /// Edit Action
    /// </summary>
    public ActionCommand EditAction { get; }

    /// <summary>
    /// Delete Action
    /// </summary>
    public ActionCommand DeleteAction { get; }

    /// <summary>
    /// Note
    /// </summary>
    public NoteModel Note
    {
        get => field;
        set => SetProperty(ref field, value);
    } = new();

    /// <summary>
    /// Notes
    /// </summary>
    public ObservableCollection<NoteModel> Notes
    {
        get => field;
        set => SetProperty(ref field, value);
    } = [];
}

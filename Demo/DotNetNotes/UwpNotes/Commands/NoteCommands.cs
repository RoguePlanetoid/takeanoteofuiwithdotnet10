namespace UwpNotes.Commands;

/// <summary>
/// Note Commands
/// </summary>
/// <param name="deleteAction">Delete Action</param>
/// <param name="editAction">Edit Action</param>
[GeneratedBindableCustomProperty]
public partial class NoteCommands(ICommand? deleteAction, ICommand? editAction) : INotifyPropertyChanged
{
    private RelayCommand? _deleteAction = deleteAction != null ? new RelayCommand(deleteAction) : null;
    private RelayCommand? _editAction = editAction != null ? new RelayCommand(editAction) : null;

    /// <summary>
    /// Property Changed
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Delete Action
    /// </summary>
    public RelayCommand? DeleteAction
    {
        get => _deleteAction;
        set
        {
            _deleteAction = value;
            PropertyChanged?.Invoke(this, 
                new PropertyChangedEventArgs(nameof(DeleteAction)));        
        }
    }

    /// <summary>
    /// Edit Action
    /// </summary>
    public RelayCommand? EditAction
    {
        get => _editAction;
        set
        {
            _editAction = value;
            PropertyChanged?.Invoke(this, 
                new PropertyChangedEventArgs(nameof(EditAction)));
        }
    }
}

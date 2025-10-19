namespace UwpNotes.Binder;

/// <summary>
/// Note View Model to Note Model Converter
/// </summary>
public partial class NoteViewModelToNoteModelConverter : IValueConverter
{
    /// <summary>
    /// Converts a <see cref="NoteViewModel"/> to a <see cref="NoteModel"/>.
    /// </summary>
    /// <param name="value">Source</param>
    /// <param name="targetType">Target Type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="language">Language</param>
    /// <returns>Note Model</returns>
    public object? Convert(object value, Type targetType, object parameter, string language) =>
        value is NoteViewModel viewModel ? NoteViewModel.AsNoteModel(viewModel) : null;

    /// <summary>
    /// Converts a <see cref="NoteModel"/> to a <see cref="NoteViewModel"/>.
    /// </summary>
    /// <param name="value">Source</param>
    /// <param name="targetType">Target Type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="language">Language</param>
    /// <returns>Note View Model</returns>
    public object? ConvertBack(object value, Type targetType, object parameter, string language) =>
        value is NoteModel model ? new NoteViewModel(model) : null; 
}
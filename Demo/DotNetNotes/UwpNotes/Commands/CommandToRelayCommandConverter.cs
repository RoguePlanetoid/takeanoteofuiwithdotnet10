namespace UwpNotes.Commands;

/// <summary>
/// Converts an <see cref="ICommand"/> to a <see cref="RelayCommand"/>.
/// </summary>
public partial class CommandToRelayCommandConverter : IValueConverter
{
    /// <summary>
    /// Converts an <see cref="ICommand"/> to a <see cref="RelayCommand"/>.
    /// </summary>
    /// <param name="value">The source <see cref="ICommand"/>.</param>
    /// <param name="targetType">The target type (should be RelayCommand).</param>
    /// <param name="parameter">Optional parameter.</param>
    /// <param name="language">Language info.</param>
    /// <returns>A <see cref="RelayCommand"/> wrapping the original <see cref="ICommand"/>.</returns>
    public object? Convert(object value, Type targetType, object parameter, string language) => 
        value is ICommand command ? new RelayCommand(command) : null;

    /// <summary>
    /// ConvertBack is not supported.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotImplementedException();
}
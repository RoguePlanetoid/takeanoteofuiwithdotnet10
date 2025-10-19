namespace WpfNotes.Controls;

/// <summary>
/// String to Brush Converter
/// </summary>
public class StringToBrushConverter : IValueConverter
{
    /// <summary>
    /// Convert
    /// </summary>
    /// <param name="value">Color String</param>
    /// <param name="targetType">Target Type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="culture">Culture</param>
    /// <returns>Solid Color Brush</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        new SolidColorBrush(value != null ? (Color)ColorConverter.ConvertFromString(value as string) : Colors.Transparent);

    /// <summary>
    /// Convert Back
    /// </summary>
    /// <param name="value">Value</param>
    /// <param name="targetType">Target Type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="culture">Culture</param>
    /// <returns>Object</returns>
    /// <exception cref="NotImplementedException">Not Implemented</exception>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}

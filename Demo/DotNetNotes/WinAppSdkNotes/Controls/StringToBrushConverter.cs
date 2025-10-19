namespace WinAppSdkNotes.Controls;

/// <summary>
/// String to Brush Converter
/// </summary>
public partial class StringToBrushConverter : IValueConverter
{
    /// <summary>
    /// Convert
    /// </summary>
    /// <param name="value">Color String</param>
    /// <param name="targetType">Target Type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="language">Language</param>
    /// <returns>Solid Color Brush</returns>
    public object Convert(object value, Type targetType, object parameter, string language) =>
        new SolidColorBrush(value != null ? Helper.FromHtml(value as string) : Colors.Transparent);

    /// <summary>
    /// Convert Back
    /// </summary>
    /// <param name="value">Value</param>
    /// <param name="targetType">Target Type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="language">Language</param>
    /// <returns>Object</returns>
    /// <exception cref="NotImplementedException">Not Implemented</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => 
        throw new NotImplementedException();
}

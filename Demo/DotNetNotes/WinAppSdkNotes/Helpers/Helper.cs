namespace WinAppSdkNotes.Helpers;

/// <summary>
/// Helper
/// </summary>
internal static class Helper
{
    /// <summary>
    /// Windows Colour from HTML Colour
    /// </summary>
    /// <param name="value">HTML Colour</param>
    /// <returns>Windows Colour</returns>
    public static Color FromHtml(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Colors.Transparent;
        value = value.TrimStart('#');
        try
        {
            var r = Convert.ToByte(value[..2], 16);
            var g = Convert.ToByte(value[2..4], 16);
            var b = Convert.ToByte(value[4..6], 16);
            return Color.FromArgb(255, r, g, b);
        }
        catch
        {
            return Colors.Transparent;
        }
    }
}
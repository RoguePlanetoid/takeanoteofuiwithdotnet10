namespace DotNetNotes.Models;

/// <summary>
/// Note Model Extensions
/// </summary>
public static class NoteModelExtensions
{
    /// <summary>
    /// Color from HTML Colour
    /// </summary>
    /// <param name="value">HTML Colour</param>
    /// <returns>Color</returns>
    internal static Color? FromHtml(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
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
            return null;
        }
    }

    // Extension Block with Extension Property
    extension(NoteModel note)
    {
        /// <summary>
        /// Asset Data URI
        /// </summary>
        public string? AssetDataUri => 
            NoteAsset.GetAsDataUri(FromHtml(note.Background), note.Title, note.Content);
    }
}

namespace DotNetNotes.Models;

/// <summary>
/// Background Model
/// </summary>
public class BackgroundModel
{
    private const string seperator = ", ";
    private const string red = "#ff4947";
    private const string orange = "#f57900";
    private const string yellow = "#ffc476";
    private const string green = "#6dc0a4";
    private const string blue = "#6f9acd";
    private const string indigo = "#833db8";
    private const string violet = "#c693c2";

    /// <summary>
    /// Values
    /// </summary>
    public static readonly List<string> Values = [red, orange, yellow, green, blue, indigo, violet];

    /// <summary>
    /// Options
    /// </summary>
    public static readonly (string Name, string Background)[] Options =
    [
        ("Red",    red),
        ("Orange", orange),
        ("Yellow", yellow),
        ("Green",  green),
        ("Blue",   blue),
        ("Indigo", indigo),
        ("Violet", violet)
    ];

    /// <summary>
    /// Names
    /// </summary>
    public static readonly List<string> Names = [.. Options.Select(p => p.Name)];

    /// <summary>
    /// Name to Background
    /// </summary>    
    public static Dictionary<string, string>? NameToBackground =>
        Options.ToDictionary(p => p.Name, p => p.Background, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Background to Name
    /// </summary>
    public static Dictionary<string, string>? BackgroundToName =>
        Options.ToDictionary(p => p.Background, p => p.Name, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Names for Output
    /// </summary>
    public static string NamesOutput => string.Join(seperator, Names);
}

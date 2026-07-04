namespace DotNetNotes.Config;

/// <summary>
/// Notes Config
/// </summary>
public class NotesConfig : INotesConfig
{
    private const string connection_string = "Data Source=notes.db";

    /// <summary>
    /// Connection String
    /// </summary>
    public string ConnectionString { get; set; } = connection_string;
}
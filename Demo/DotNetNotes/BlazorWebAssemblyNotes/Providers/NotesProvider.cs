namespace BlazorWebAssemblyNotes.Providers;

/// <summary>
/// Web Assembly Notes Provider
/// </summary>
/// <param name="localStorage">Local Storage</param>
public class NotesProvider(ILocalStorageService localStorage) : INotesProvider
{
    private const string key = "notes.db";

    /// <summary>
    /// Get Collection
    /// </summary>
    /// <returns>List of Note Model</returns>
    private async Task<List<NoteModel>> GetCollection() => 
        await localStorage.ContainKeyAsync(key) ? 
            await localStorage.GetItemAsync<List<NoteModel>>(key) ?? [] : [];

    /// <summary>
    /// Create Storage
    /// </summary>
    /// <returns>True on Success, False if Not</returns>
    public async Task<bool> CreateAsync()
    {
        try
        {
            if (!await localStorage.ContainKeyAsync(key))
                await localStorage.SetItemAsync(key, new List<NoteModel>());
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Add
    /// </summary>
    /// <param name="note">Note</param>
    /// <returns>Id</returns>
    public async Task<int?> AddAsync(NoteModel note)
    {
        try
        {
            var notes = await GetCollection();            
            int nextId = notes.Count > 0 ? notes.Max(n => n.Id ?? 0) + 1 : 1;
            note.Id = nextId;
            notes.Add(note);
            await localStorage.SetItemAsync(key, notes);
            return note.Id;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Get
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Note</returns>
    public async Task<NoteModel?> GetAsync(int id)
    {
        try
        {
            var notes = await GetCollection();
            return notes.FirstOrDefault(n => n.Id == id);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// List
    /// </summary>
    /// <returns>IEnumerable of Note</returns>
    public async Task<IEnumerable<NoteModel>> ListAsync()
    {
        try
        {
            return await GetCollection();
        }
        catch
        {
            return [];
        }
    }

    /// <summary>
    /// Edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="note">Note</param>
    /// <returns>True on Success, False if Not</returns>
    public async Task<bool> EditAsync(int id, NoteModel note)
    {
        try
        {
            var notes = await GetCollection();
            var existingNoteIndex = notes.FindIndex(n => n.Id == id);
            if (existingNoteIndex == -1)
                return false;
            note.Id = id;
            notes[existingNoteIndex] = note;
            await localStorage.SetItemAsync(key, notes);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>True on Success, False if Not</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var notes = await GetCollection();
            var existingNote = notes.FirstOrDefault(n => n.Id == id);
            if (existingNote == null)
                return false;
            notes.Remove(existingNote);
            await localStorage.SetItemAsync(key, notes);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Destroy Storage
    /// </summary>
    /// <returns>True on Success, False if Not</returns>
    public async Task<bool> DestroyAsync()
    {
        try
        {
            await localStorage.RemoveItemAsync(key);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
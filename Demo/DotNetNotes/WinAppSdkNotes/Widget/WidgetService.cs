namespace WinAppSdkNotes.Widget;

/// <summary>
/// Widget Service
/// </summary>
/// <param name="file">File Provider</param>
/// <param name="application">Application Provider</param>
public class WidgetService(FileProvider file, IApplicationProvider application)
{
    private const int page_size = 3;
    private const string id = "noteId"; 
    private const string data = "note.json";
    
    private WidgetModel _widget = new();

    /// <summary>
    /// Get Image
    /// </summary>
    /// <param name="model">Note Model</param>
    /// <returns>Note Image</returns>
    private static string GetImage(NoteModel model) =>
        model.AssetDataUri ?? string.Empty;

    /// <summary>
    /// Note Does Not Exist
    /// </summary>
    /// <param name="note">Note</param>
    /// <param name="notes">Notes</param>
    /// <returns>True if Note does not Exist, False if Does</returns>
    private static bool NoteDoesNotExist(int? id, List<NoteModel> notes) =>
        id == null || notes.Any(a => a.Id == id) == false;

    /// <summary>
    /// Populate
    /// </summary>    
    private void Populate()
    {
        application.ListAsync().Wait();
        var note = application.Content.Note;
        var notes = application.Content.Notes.ToList();
        _widget.Total = notes.Count;
        _widget.Page = _widget.Page > (int)Math.Ceiling((double)_widget.Total / page_size) ? 1 : _widget.Page;
        _widget.Notes = [.. notes.Skip((_widget.Page - 1) * page_size).Take(page_size)];
        _widget.TotalPages = (int)Math.Ceiling((double)_widget.Total / page_size);
        _widget.Note = NoteDoesNotExist(note?.Id, notes) ? notes.FirstOrDefault() : note;
        _widget.Image = NoteDoesNotExist(_widget.ImageId, notes) ? null : _widget.Image;
        _widget.Note = note;
        file?.Save(data, _widget);
    }

    /// <summary>
    /// Get Note Id
    /// </summary>
    /// <param name="json">Json</param>
    /// <returns>Note Id</returns>
    private static int? GetNoteId(string json)
    {
        try
        {
            _ = int.TryParse(JsonObject.Parse(json).GetNamedString(id, null), out var result);
            return result;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Get Note Model
    /// </summary>
    /// <param name="json">Json</param>
    /// <returns>Widget Model</returns>
    private static NoteModel? GetNoteModel(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<NoteModel>(json);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Set Image
    /// </summary>
    /// <param name="model">Note Model</param>
    private void SetImage(NoteModel? model)
    {
        if (model != null)
        {
            _widget.ImageId = model.Id; 
            _widget.Image = GetImage(model);
        }
    }

    /// <summary>
    /// New Note
    /// </summary>
    /// <param name="model">Note Model</param>
    private void NewNote(NoteModel model)
    {
        var result = application.NewAsync(model, true).Result;
        if (result)
        {
            SetImage(application.Content.Note);
            Populate();
        }
    }

    /// <summary>
    /// Edit Note
    /// </summary>
    /// <param name="model">Note Model</param>
    private void EditNote(NoteModel model)
    {
        var result = application.EditAsync(model, true).Result;
        if (result)
        {
            SetImage(application.Content.Note);
            Populate();
        }
    }

    /// <summary>
    /// Start
    /// </summary>
    public void Start()
    {
        _widget.Page = 1;
        Populate();
    }

    /// <summary>
    /// Get Data
    /// </summary>
    /// <returns>Widget Data</returns>
    public string GetData() =>
        file!.ToJson(_widget);

    /// <summary>
    /// Load Data
    /// </summary>
    /// <returns>Widget Data</returns>
    public string LoadData()
    {
        _widget = file!.Load<WidgetModel>(data) ?? new();
        Populate();
        return GetData();
    }

    /// <summary>
    /// Get Note
    /// </summary>
    /// <param name="data">Note Id Data</param>
    public void Get(string data)
    {
        application.Content.Note = application.GetAsync(GetNoteId(data)).Result ?? application.Content.Note;
        Populate();
    }

    /// <summary>
    /// Select Note
    /// </summary>
    /// <param name="data">Note Id Data</param>
    public void Select(string data)
    {
        SetImage(application.GetAsync(GetNoteId(data)).Result);
        Populate();
    }

    /// <summary>
    /// New
    /// </summary>
    public void New()
    {
        application.Content.Note = new();
        Populate();
    }

    /// <summary>
    /// Upsert
    /// </summary>
    /// <param name="data">Note Model Data</param>
    public void Upsert(string data)
    {
        var model = GetNoteModel(data);
        if(model?.Id == null)
            NewNote(model ?? new NoteModel());
        else
            EditNote(model);
    }

    /// <summary>
    /// Delete Note
    /// </summary>
    /// <param name="data">Note Model Data</param>
    public void Delete(string data)
    {
        var note = GetNoteModel(data);
        if (note != null)
        {
            var result = application.DeleteAsync(note, true).Result;
            if (result)
                Populate();
        }
    }

    /// <summary>
    /// Next
    /// </summary>
    public void Next()
    {
        if (_widget.Page < _widget.TotalPages)
        {
            _widget.Page++;
            Populate();
        }
    }

    /// <summary>
    /// Next
    /// </summary>
    public void Prev()
    {
        if (_widget.Page > 1)
        {
            _widget.Page--;
            Populate();
        }
    }

    /// <summary>
    /// Is Image Empty
    /// </summary>
    /// <returns>True if Image is Empty, False if not</returns>
    public bool IsImageEmpty => _widget.IsImageEmpty;
}

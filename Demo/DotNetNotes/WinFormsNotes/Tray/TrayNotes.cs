namespace WinFormsNotes.Tray;

/// <summary>
/// Tray Notes Provider
/// </summary>
public sealed class TrayNotes : IDisposable
{
    private const int notes = 10;
    private const int length = 40;
    private const string ellipsis = "...";
    private const string tray_new = "New";
    private const string tray_show = "Show";
    private const string tray_exit = "Exit";
    private const string tray_edit = "Edit";
    private const string tray_delete = "Delete";
    private const string tray_refresh = "Refresh";
    private const string tray_title = "Tray Notes";
    private const string untitled_note = "(Untitled)";
    private const string icon = "WinFormsNotes.Note.ico";

    private readonly IApplicationProvider _application;
    private readonly WinFormsDialog _dialog;
    private readonly MainForm _form;
    private readonly NotifyIcon _notifyIcon;
    private readonly ContextMenuStrip _menu;
    private readonly ToolStripMenuItem _show;
    private readonly ToolStripMenuItem _new;
    private readonly ToolStripMenuItem _refresh;
    private readonly ToolStripSeparator _showSep;
    private readonly ToolStripSeparator _noteSep;
    private readonly ToolStripSeparator _exitSep;
    private readonly ToolStripMenuItem _exit;
    private bool _disposed;

    /// <summary>
    /// Load Icon
    /// </summary>
    /// <returns>Icon</returns>
    private static Icon LoadIcon()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(icon);
        return stream is not null ? new Icon(stream) : SystemIcons.Application;
    }

    /// <summary>
    /// Exit
    /// </summary>
    private static void Exit() =>
        Application.ExitThread();

    /// <summary>
    /// Reload
    /// </summary>
    private async Task Reload()
    {
        await _application.ListAsync();
        RefreshNotes();
    }

    /// <summary>
    /// Truncate
    /// </summary>
    private static string Truncate(string value, int length) =>
        value.Length <= length ? value : value[..(length - 3)] + ellipsis;

    /// <summary>
    /// Build Note Menu Item
    /// </summary>
    /// <param name="note">Note</param>
    /// <returns>ToolStripMenuItem</returns>
    private ToolStripMenuItem BuildNoteMenuItem(NoteModel note)
    {
        var title = string.IsNullOrWhiteSpace(note.Title) ? untitled_note : 
            Truncate(note.Title, length);   
        var item = new ToolStripMenuItem(title) { Tag = note };
        var edit = new ToolStripMenuItem(tray_edit, null, async (_, _) =>
            await _application.EditAsync(note));
        var delete = new ToolStripMenuItem(tray_delete, null, async (_, _) =>
            await _application.DeleteAsync(note));
        item.DropDownItems.Add(edit);
        item.DropDownItems.Add(delete);
        return item;
    }

    /// <summary>
    /// Refresh Notes
    /// </summary>
    private void RefreshNotes()
    {
        if (_disposed)
            return;
        bool isNotes = false;
        var remove = new List<ToolStripItem>();
        foreach (ToolStripItem item in _menu.Items)
        {
            if (item == _noteSep)
            {
                isNotes = true;
                continue;
            }
            if (item == _exitSep)
                break;
            if (isNotes)
                remove.Add(item);
        }
        foreach (var item in remove)
            _menu.Items.Remove(item);
        int index = _menu.Items.IndexOf(_exitSep);
        foreach (var note in _application.Content.Notes.Take(notes))
            _menu.Items.Insert(index++, BuildNoteMenuItem(note));
        _noteSep.Visible = _application.Content.Notes.Any();
    }

    /// <summary>
    /// Update Tray
    /// </summary>
    private async void UpdateTray()
    {
        if (_disposed) 
            return;
        var show = !_form.Visible;
        if (_notifyIcon.Visible != show)
            _notifyIcon.Visible = show;
        _show.Visible = show;
        if (show)
            await Reload();
    }

    /// <summary>
    /// Show Form and Hide Tray
    /// </summary>
    private void Show()
    {
        if (_disposed) 
            return;
        if (!_form.Visible)
            _form.Show();
        if (_form.WindowState == FormWindowState.Minimized)
            _form.WindowState = FormWindowState.Maximized;
        _form.Activate();
        UpdateTray();
    }

    /// <summary>
    /// Hide Form and Show Tray
    /// </summary>
    private void HideForm()
    {
        if (_disposed) return;
        if (_form.Visible)
            _form.Hide();
        UpdateTray();
    }

    /// <summary>
    /// Form Closing
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event Args</param>
    private void FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_disposed) 
            return;
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            HideForm();
        }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="application">Application provider</param>
    /// <param name="dialog">Dialog service</param>
    /// <param name="form">Form</param>
    public TrayNotes(IApplicationProvider application, WinFormsDialog dialog, MainForm form)
    {
        _application = application;
        _dialog = dialog;
        _form = form;
        _menu = new ContextMenuStrip();
        _show = new(tray_show, null, (_, _) => 
            Show());
        _refresh = new(tray_refresh, null, async (_, _) =>
            await Reload());
        _new = new(tray_new, null, async (_, _) => 
            await _application.NewAsync());
        _exit = new(tray_exit, null, (_, _) =>
            Exit());
        _showSep = new();
        _noteSep = new();
        _exitSep = new();
        _menu.Items.AddRange([ _show, _showSep, _refresh, _new, _noteSep, _exitSep, _exit ]);
        _notifyIcon = new NotifyIcon
        {            
            ContextMenuStrip = _menu,
            Text = tray_title,
            Icon = LoadIcon(),
            Visible = false    
        };
        _notifyIcon.DoubleClick += (_, _) => 
            Show();
        application.Confirm = WinFormsDialog.DeleteAsync;
        application.Upsert = async (DialogModel model) => 
            await _dialog.UpsertAsync(form, model);
        application.Content.Notes.CollectionChanged += (_, _) => 
            RefreshNotes();
        application.Updated = 
            RefreshNotes;
        _form.FormClosing += FormClosing;
        _form.VisibleChanged += (_, _) => 
            UpdateTray();
        _form.Resize += (_, _) =>
        {
            if (_form.WindowState == FormWindowState.Minimized)
                HideForm();
        };
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        if (_disposed) 
            return;
        _disposed = true;
        try
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            _menu.Dispose();
        }
        catch
        {
            // Ignore Exception
        }
    }
}

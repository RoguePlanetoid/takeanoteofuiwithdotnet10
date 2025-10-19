namespace WinFormsNotes;

/// <summary>
/// Main Form
/// </summary>
public partial class MainForm : Form
{
    private readonly IApplicationProvider _application;

    /// <summary>
    /// Trigger Action on UI Thread
    /// </summary>
    /// <param name="action">Action</param>
    private void Trigger(Action action)
    {
        if (IsHandleCreated && InvokeRequired)
            BeginInvoke(action);
        else
            action();
    }

    /// <summary>
    /// Get Control for Note
    /// </summary>
    /// <param name="note">Note Model</param>
    /// <returns>Note Control</returns>
    private Control GetControl(NoteModel note)
    {
        var control = new NoteControl
        {
            Tag = note,
            Title = note.Title,
            Size = new(600, 600),
            Content = note.Content,
            Fill = ColorTranslator.FromHtml(note.Background)
        };
        control.MouseUp += async (s, e) =>
        {
            if (e.Button == MouseButtons.Left)
                await _application.EditAsync((s as NoteControl)?.Tag as NoteModel);
            else if (e.Button == MouseButtons.Right)
                await _application.DeleteAsync((s as NoteControl)?.Tag as NoteModel);
        };
        return control;
    }

    /// <summary>
    /// Refresh Notes
    /// </summary>
    private void RefreshNotes()
    {
        Display.SuspendLayout();
        try
        {
            Display.Controls.Clear();
            Display.Controls.AddRange([.. _application.Content.Notes.Select(GetControl)]);
        }
        finally
        {
            Display.ResumeLayout();
        }
    }

    /// <summary>
    /// Reload
    /// </summary>    
    private async Task Reload()
    {
        await _application.ListAsync();
        RefreshNotes();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="application">Application</param>
    /// <param name="dialog">Dialog</param>
    public MainForm(IApplicationProvider application, WinFormsDialog dialog)
    {
        _application = application;
        InitializeComponent();
        application.Confirm = WinFormsDialog.DeleteAsync;
        application.Upsert = async (DialogModel model) =>
            await dialog.UpsertAsync(this, model);
        New.Click += async (_, _) =>
            await application.NewAsync();
        List.Click += async (_, _) =>
            await Reload();
        _application.Updated = () =>
            Trigger(RefreshNotes);
        _application.Content.Notes.CollectionChanged += (_, _) =>
            Trigger(RefreshNotes);
        Load += (_, _) =>
            Trigger(RefreshNotes);
        Resize += (_, _) =>
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();
        };
        VisibleChanged += async (_, _) =>
        {
            if (Visible)
                await Reload();
        };
    }

    /// <summary>
    /// On Form Closing
    /// </summary>
    /// <param name="e">Event Args</param>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            Hide();
            return;
        }
        base.OnFormClosing(e);
    }
}

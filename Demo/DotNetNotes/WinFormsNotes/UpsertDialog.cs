namespace WinFormsNotes;

/// <summary>
/// Upsert Dialog
/// </summary>
public partial class UpsertDialog : Form
{
    private const int preview = 100;

    /// <summary>
    /// Dialog Model
    /// </summary>
    public DialogModel Model { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="model">Dialog Model</param>
    public UpsertDialog(DialogModel model)
    {
        InitializeComponent();
        Model = model;
        Text = model.Title;
        Title.Text = model.Note.Title;
        Content.Text = model.Note.Content;
        Background.DrawMode = DrawMode.OwnerDrawFixed;
        Background.DropDownStyle = ComboBoxStyle.DropDownList;
        Background.ItemHeight = preview;
        Background.DrawItem += Background_DrawItem;
        Background.DataSource = model.Colours;
        Background.SelectedItem = model.Note.Background;
        PrimaryButton.Text = model.PrimaryOption;
        SecondaryButton.Text = model.SecondaryOption;
    }

    /// <summary>
    /// Background Draw Item
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event Args</param>
    private void Background_DrawItem(object? sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        e.DrawFocusRectangle();
        if (e.Index < 0 || e.Index >= Background.Items.Count)
            return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        var background = Background.Items[e.Index]?.ToString() ?? string.Empty;
        var fill = ColorTranslator.FromHtml(background);
        int side = Math.Min(e.Bounds.Height - 6, e.Bounds.Width - 12);
        int x = e.Bounds.X + (e.Bounds.Width - side) / 2;
        int y = e.Bounds.Y + (e.Bounds.Height - side) / 2;
        var swatchRect = new Rectangle(x, y, side, side);
        var control = new NoteControl()
        {
            Fill = fill,
            Size = new Size(side, side),
            Location = new Point(0, 0)
        };
        using var bitmap = new Bitmap(side, side);
        control.DrawToBitmap(bitmap, new Rectangle(0, 0, side, side));
        e.Graphics.DrawImage(bitmap, swatchRect);
    }

    /// <summary>
    /// Primary Button Click
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event Arguments</param>
    private void PrimaryButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Title.Text) && !string.IsNullOrEmpty(Content.Text))
        {
            Model.Note.Title = Title.Text;
            Model.Note.Content = Content.Text;
            Model.Note.Background = Background.SelectedItem?.ToString() ?? Model.Note.Background;
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    /// <summary>
    /// Secondary Button Click
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event Arguments</param>
    private void SecondaryButton_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}

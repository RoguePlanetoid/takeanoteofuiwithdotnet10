namespace WinFormsNotes.Controls;

/// <summary>
/// Note Control
/// </summary>
public class NoteControl : Control
{
    private const string note_font = "Segoe Script";
    private const float title_ratio = 0.15f;

    private readonly Font title_font = new(note_font, 14f);
    private readonly Font content_font = new(note_font, 12f);

    private Color _fill = Color.Transparent;
    private string _title = string.Empty;
    private string _content = string.Empty;

    /// <summary>
    /// On Paint
    /// </summary>
    /// <param name="e">Event Args</param>
    protected override void OnPaint(PaintEventArgs e)
    {
        // Set Graphics Quality
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        // Draw Note
        using (var path = CreateNotePath())
        {
            using var fillBrush = new SolidBrush(_fill);
            e.Graphics.FillPath(fillBrush, path);
        }
        // Draw Folded Corner
        using (var cornerPath = CreateCornerPath())
        {
            using var cornerBrush = new SolidBrush(Color.FromArgb(80, 255, 255, 255));
            e.Graphics.FillPath(cornerBrush, cornerPath);
        }
        // Draw Title Shadow
        var titleHeight = Height * title_ratio;
        var titleRect = new RectangleF(0, 0, Width, titleHeight);
        using (var shadowBrush = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
        {
            e.Graphics.FillRectangle(shadowBrush, titleRect);
        }
        // Draw Title
        using (var format = new StringFormat())
        {
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(Title, title_font, Brushes.Black, titleRect, format);
        }
        // Draw Content
        var contentRect = new RectangleF(5, titleHeight, Width - 10, Height - titleHeight - 5);
        using (var format = new StringFormat())
        {
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Near;
            e.Graphics.DrawString(Content, content_font, Brushes.Black, contentRect, format);
        }
        base.OnPaint(e);
    }

    /// <summary>
    /// Create outside folded corner path using shape with diagonal line at bottom-right
    /// </summary>
    /// <returns>Graphics Path</returns>
    private GraphicsPath CreateNotePath()
    {
        var path = new GraphicsPath();
        float w = Width;
        float h = Height;
        float foldSize = w * 0.25f;
        path.AddLine(0, 0, w, 0); // Top edge
        path.AddLine(w, 0, w, h - foldSize); // Right edge up to fold start
        path.AddLine(w, h - foldSize, w - foldSize, h); // Diagonal fold edge
        path.AddLine(w - foldSize, h, 0, h); // Bottom edge
        path.CloseFigure();
        return path;
    }

    /// <summary>
    /// Create inside folded corner path using triangle
    /// </summary>
    /// <returns>Graphics Path</returns>
    private GraphicsPath CreateCornerPath()
    {
        var path = new GraphicsPath();
        float w = Width;
        float h = Height;
        float foldSize = w * 0.25f;      
        path.AddLine(w, h - foldSize, w - foldSize, h); // Diagonal
        path.AddLine(w - foldSize, h, w - foldSize, h - foldSize); // Upwards
        path.CloseFigure();
        return path;
    }

    /// <summary>
    /// Dispose
    /// </summary>
    /// <param name="disposing">Disposing</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            title_font?.Dispose();
            content_font?.Dispose();
        }
        base.Dispose(disposing);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public NoteControl()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.UserPaint |
            ControlStyles.ResizeRedraw, true);
    }

    /// <summary>
    /// Title
    /// </summary>
    [Category("Appearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Content
    /// </summary>
    [Category("Appearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Content
    {
        get => _content;
        set
        {
            if (_content != value)
            {
                _content = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Fill
    /// </summary>
    [Category("Appearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color Fill
    {
        get => _fill;
        set
        {
            if (_fill != value)
            {
                _fill = value;
                Invalidate();
            }
        }
    }
}
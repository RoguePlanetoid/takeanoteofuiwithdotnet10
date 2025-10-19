namespace MauiNotes.Controls;

/// <summary>
/// Note Control
/// </summary>
public partial class NoteControl : Grid
{
    private const string note_font = "Calligraffitti";
    private const double default_size = 466;

    internal Grid _grid;

    /// <summary>
    /// Get Path
    /// </summary>
    /// <param name="value">String</param>
    /// <returns>Path</returns>
    private static Path GetPath(string value) => 
        new Path().LoadFromXaml(
        @$"<Path xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
            <Path.Data>{value}</Path.Data>
        </Path>");

    /// <summary>
    /// Layout
    /// </summary>
    private void Layout()
    {
        var currentSize = Size;
        var scaleFactor = currentSize / default_size;
        var path = GetPath(NoteAsset.GetPrimaryPathMarkup());
        path.WidthRequest = currentSize;
        path.HeightRequest = currentSize;
        path.SetBinding(Shape.FillProperty, new Binding()
        {
            Mode = BindingMode.TwoWay,
            Path = nameof(Fill),
            Source = this
        });
        path.RenderTransform = new ScaleTransform()
        {
            ScaleX = scaleFactor,
            ScaleY = scaleFactor,
        };
        var corner = GetPath(NoteAsset.GetSecondaryPathMarkup());
        corner.WidthRequest = currentSize;
        corner.HeightRequest = currentSize;
        corner.Fill = Color.FromRgba(255, 255, 255, 80);
        corner.RenderTransform = new ScaleTransform()
        {
            ScaleX = scaleFactor,
            ScaleY = scaleFactor,
        };
        var note = new Grid()
        {
            HeightRequest = currentSize,
            WidthRequest = currentSize
        };        
        note.Children.Add(path);
        note.Children.Add(corner);
        note.SetValue(RowSpanProperty, 2);
        _grid.Children.Add(note);
        var rectangle = new Rectangle()
        {
            Fill = Color.FromRgba(0, 0, 0, 40),
            Aspect = Stretch.UniformToFill,
            WidthRequest = currentSize
        };
        rectangle.SetValue(RowProperty, 0);
        _grid.Children.Add(rectangle);
        var title = new Label()
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = currentSize * 0.073,
            Margin = new Thickness(2),
            FontFamily = note_font
        };
        title.SetBinding(Label.TextProperty, new Binding()
        {
            Mode = BindingMode.TwoWay,
            Path = nameof(Title),
            Source = this
        });
        title.SetValue(RowProperty, 0);
        _grid.Children.Add(title);
        var content = new Label()
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            FontSize = currentSize * 0.064,
            Margin = new Thickness(16),
            FontFamily = note_font            
        };
        content.SetBinding(Label.TextProperty, new Binding()
        {
            Mode = BindingMode.TwoWay,
            Path = nameof(Content),
            Source = this
        });
        content.SetValue(RowProperty, 1);
        _grid.Children.Add(content);
    }

    /// <summary>
    /// Update Layout based on Size
    /// </summary>
    private void UpdateLayout()
    {
        if (_grid != null)
        {
            _grid.HeightRequest = Size;
            _grid.WidthRequest = Size;
            _grid.Children.Clear();
            Layout();
        }
    }

    /// <summary>
    /// On Size Changed
    /// </summary>
    /// <param name="bindable">Object</param>
    /// <param name="oldValue">New Value</param>
    /// <param name="newValue">Old Value</param>
    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NoteControl control)
            control.UpdateLayout();
    }

    /// <summary>
    /// Title Property
    /// </summary>
    public static readonly BindableProperty TitleProperty = 
        BindableProperty.Create(nameof(Title), typeof(string), typeof(NoteControl), null);

    /// <summary>
    /// Content Property
    /// </summary>
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(nameof(Content), typeof(string), typeof(NoteControl), null);

    /// <summary>
    /// Fill Property
    /// </summary>
    public static readonly BindableProperty FillProperty =
        BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(NoteControl), 
        new SolidColorBrush(Colors.Transparent));

    /// <summary>
    /// Size Property
    /// </summary>
    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(double), typeof(NoteControl), 
        default_size, propertyChanged: OnSizeChanged);

    /// <summary>
    /// Title
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Content
    /// </summary>
    public string Content
    {
        get => (string)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// Fill
    /// </summary>
    public Brush Fill
    {
        get => (Brush)GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    /// <summary>
    /// Size
    /// </summary>
    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public NoteControl()
    {
        _grid = new Grid()
        {
            HeightRequest = default_size,
            WidthRequest = default_size
        };
        _grid.RowDefinitions.Add(new RowDefinition(new GridLength(0.15, GridUnitType.Star)));
        _grid.RowDefinitions.Add(new RowDefinition(new GridLength(0.85, GridUnitType.Star)));
        Layout();
        Children.Add(_grid);
    }
}

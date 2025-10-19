namespace MauiNotes;

/// <summary>
/// Dialog Popup
/// </summary>
public partial class DialogPopup : Popup
{
    /// <summary>
    /// Constructor
    /// </summary>
    public DialogPopup() => 
        InitializeComponent();

    /// <summary>
    /// Result
    /// </summary>
    public bool? Result { get; private set; } 

    /// <summary>
    /// Primary Option
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event</param>
    private async void PrimaryOption(object sender, EventArgs e)
    {
        if (BindingContext is DialogModel model && model.IsValid)
        {
            Result = true;
            await CloseAsync();
        }
    }

    /// <summary>
    /// Secondary Option
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event</param>
    private async void SecondaryOption(object sender, EventArgs e)
    {
        Result = false;
        await CloseAsync();
    }
}
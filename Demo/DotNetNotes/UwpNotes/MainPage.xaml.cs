using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace UwpNotes;

/// <summary>
/// An empty page that can be used on its own or navigated to within a <see cref="Frame">.
/// </summary>
public sealed partial class MainPage : Page
{
    private readonly IApplicationProvider? application;
    private readonly UwpDialog? dialog;

    /// <summary>
    /// Constructor
    /// </summary>
    [RequiresUnreferencedCode("Calls IApplicationProvider.NewAsync which may break when trimming.")]
    public MainPage()
    {
        InitializeComponent();
        application = App.Host?.Services.GetRequiredService<IApplicationProvider>();
        dialog = App.Host?.Services.GetRequiredService<UwpDialog>();
        application!.Confirm = async (DialogModel model) =>
        {
            DialogModel = model;
            Bindings.Update();
            return await UwpDialog.DeleteAsync(DeleteDialog);
        };
        application!.Upsert = async (DialogModel model) =>
        {
            var background = model.Note.Background;
            DialogModel = model;
            Bindings.Update();
            DialogModel.Note.Background = background;
            return await dialog!.UpsertAsync(UpsertDialog, model);
        };
        application.Updated = this.Bindings.Update;
        ViewModel = application.Content;
        Notes = new ObservableCollection<NoteViewModel>(
            application!.Content.Notes.Select(s => new NoteViewModel(s)));
        application!.Content.Notes.CollectionChanged += (s, e) =>
        {
            if (e.NewItems != null)
            {
                foreach (NoteModel target in e.NewItems)
                {
                    var original = Notes.FirstOrDefault(w => w.Id == target.Id);
                    if (original == null)
                        Notes.Add(new(target));
                    else
                        Notes[Notes.IndexOf(original)] = new(target);
                }
            }
            if (e.OldItems != null)
            {
                foreach (NoteModel target in e.OldItems)
                {
                    var original = Notes.FirstOrDefault(w => w.Id == target.Id);
                    if (original != null)
                        Notes.Remove(original);
                }
            }
            Bindings.Update();
        };
        DataContext = new NoteCommands(
            application.Content.DeleteAction.Command,
            application.Content.EditAction.Command);
        Bindings.Update();
    }

    /// <summary>
    /// Content Model
    /// </summary>
    public ContentModel? ViewModel { get; set; }

    /// <summary>
    /// Dialog Model
    /// </summary>
    public DialogModel? DialogModel { get; set; }

    /// <summary>
    /// Note View Models
    /// </summary>
    public ObservableCollection<NoteViewModel> Notes { get; set; } = [];
}

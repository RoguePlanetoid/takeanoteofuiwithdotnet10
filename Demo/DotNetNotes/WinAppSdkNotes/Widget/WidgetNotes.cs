namespace WinAppSdkNotes.Widget;

/// <summary>
/// Widget Notes
/// </summary>
/// <param name="provider">Widget Notes Provider</param>
/// <param name="widgetId">Widget Id</param>
/// <param name="initialState">Initial State</param>
public class WidgetNotes(WidgetService provider, string widgetId, string initialState) : WidgetBase(widgetId, initialState)
{
    private const string new_note = "new";
    private const string edit_note = "edit";
    private const string save_note = "save";
    private const string next_notes = "next";
    private const string prev_notes = "prev";    
    private const string select_note = "select";
    private const string delete_note = "delete";
    private const string cancel_note = "cancel";
    private const string confirm_note = "confirm";
    private const string refresh_notes = "refresh";

    private const string note = "ms-appx:///Widget/Resources/Note.json";    
    private const string upsert = "ms-appx:///Widget/Resources/Upsert.json";
    private const string confirm = "ms-appx:///Widget/Resources/Confirm.json";
    private const string configure = "ms-appx:///Widget/Resources/Configure.json";
    private static readonly string note_template = WidgetHelper.ReadJsonFromPackage(note);
    private static readonly string upsert_template = WidgetHelper.ReadJsonFromPackage(upsert);
    private static readonly string confirm_template = WidgetHelper.ReadJsonFromPackage(confirm);
    private static readonly string configure_template = WidgetHelper.ReadJsonFromPackage(configure);

    private WidgetTemplate _template = WidgetTemplate.None;

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        SetState(provider.GetData());
        var options = new WidgetUpdateRequestOptions(Id)
        {
            Data = GetDataForWidget(),
            Template = GetTemplateForWidget(),
            CustomState = State
        };
        WidgetManager.GetDefault().UpdateWidget(options);
    }

    /// <summary>
    /// Get Template
    /// </summary>
    /// <returns>Widget Tenmplate</returns>
    private WidgetTemplate GetTemplate() =>
        provider.IsSelectedEmpty == true ? WidgetTemplate.Configure : WidgetTemplate.Note;

    /// <summary>
    /// Get Template Data
    /// </summary>
    /// <returns>Template Data</returns>
    private string GetTemplateData() => 
        provider.IsSelectedEmpty == true ? configure_template : note_template;

    /// <summary>
    /// Definition Id
    /// </summary>
    public static string DefinitionId { get; } = nameof(WidgetNotes);

    /// <summary>
    /// On Action Invoked
    /// </summary>
    /// <param name="actionInvokedArgs">Action Invoked Args</param>
    public override void OnActionInvoked(WidgetActionInvokedArgs actionInvokedArgs)
    {
        switch (actionInvokedArgs.Verb)
        {
            case confirm_note:
                provider.Delete(actionInvokedArgs.Data);
                _template = WidgetTemplate.Configure;
                break;
            case save_note:
                provider.Upsert(actionInvokedArgs.Data);
                _template = GetTemplate();
                break;
            case edit_note:
                provider.Get(actionInvokedArgs.Data);
                _template = WidgetTemplate.Upsert;
                break;
            case select_note:
                provider.Select(actionInvokedArgs.Data);
                _template = WidgetTemplate.Note;
                break;
            case delete_note:                               
                provider.Get(actionInvokedArgs.Data);               
                _template = WidgetTemplate.Confirm;
                break;
            case new_note:
                provider.New();
                _template = WidgetTemplate.Upsert;
                break;
            case cancel_note:
                _template = WidgetTemplate.Configure;
                break;
            case prev_notes:
                provider.Prev();
                _template = WidgetTemplate.Configure;
                break;
            case next_notes:
                provider.Next();
                _template = WidgetTemplate.Configure;
                break;
            case refresh_notes:
                provider.LoadData();
                _template = WidgetTemplate.Configure;
                break;            
            default:
                break;
        }
        Update();
    }

    /// <summary>
    /// On Customization Requested
    /// </summary>
    /// <param name="customizationRequestedArgs">Customization Requested Args</param>
    public override void OnCustomizationRequested(
        WidgetCustomizationRequestedArgs customizationRequestedArgs)
    {
        _template = WidgetTemplate.Configure;
        provider.Start();
        Update();
    }
    
    /// <summary>
    /// Get Data for Widget
    /// </summary>
    /// <returns>Widget Data</returns>
    public override string GetDataForWidget() => 
        provider.LoadData();

    /// <summary>
    /// Get Template for Widget
    /// </summary>
    /// <returns>Widget Template</returns>
    public override string GetTemplateForWidget() => 
        _template switch
        {
            WidgetTemplate.Configure => configure_template,
            WidgetTemplate.Confirm => confirm_template,    
            WidgetTemplate.Upsert => upsert_template,
            WidgetTemplate.Note => note_template,
            _ => GetTemplateData()
        };
}

namespace ConsoleNotes.Providers;

/// <summary>
/// Console Provider
/// </summary>
internal class ConsoleProvider
{
    // Commands
    private const string new_command = "new";
    private const string edit_command = "edit";
    private const string get_command = "get";
    private const string list_command = "list";
    private const string help_command = "help";
    private const string delete_command = "delete";
    // Arguments
    private const string id_argument = "id";
    private const string title_argument = "title";
    private const string confirm_argument = "confirm";
    private const string content_argument = "content";
    private const string background_argument = "background";

    private readonly IApplicationProvider _application;

    private ConsoleAction _action = ConsoleAction.None;
    private string? _id;
    private string? _title;
    private string? _content;
    private string? _background;
    private bool? _confirm;
    private bool _hasAction;
    private bool _hasConflict;
    
    /// <summary>
    /// Try Parse Id
    /// </summary>
    /// <param name="value">Value</param>
    /// <returns>Id</returns>
    private static int? TryParse(string? value) =>
        int.TryParse(value, out var id) ? id : null;

    /// <summary>
    /// All Values Specified
    /// </summary>
    /// <param name="values">Values</param>
    /// <returns>True if all values are specified, otherwise False</returns>
    private static bool AllSpecified(params string?[] values) =>
        values.All(v => !string.IsNullOrWhiteSpace(v));

    /// <summary>
    /// Add Action If Present in Argument Map
    /// </summary>
    /// <param name="map">Argument Map</param>
    /// <param name="key">Argument Key</param>
    /// <param name="list">Action List</param>
    /// <param name="action">Action</param>
    private static void AddActionIfPresent(Dictionary<string, string> map,
        string key, List<(ConsoleAction, string?)> list, ConsoleAction action)
    {
        if (map.TryGetValue(key, out var val))
            list.Add((action, val));
    }

    /// <summary>
    /// Output message to console in specified colour
    /// </summary>
    /// <param name="colour">Colour</param>
    /// <param name="message">Message</param>
    /// <param name="inline">Is Message Inline?</param>
    private static void Output(ConsoleColor colour, string message, bool inline = false)
    {
        var original = Console.ForegroundColor;
        Console.ForegroundColor = colour;
        if (inline)
            Console.Write(message);
        else
            Console.WriteLine(message);
        Console.ForegroundColor = original;
    }

    /// <summary>
    /// Gets background output
    /// </summary>
    /// <param name="background">Background</param>
    /// <returns>Output</returns>
    private static string GetBackgroundOutput(string background) =>
        BackgroundModel.BackgroundToName?.TryGetValue(background, out var name) == true ? $"{name}" : background;

    /// <summary>
    /// Writes help information to the console
    /// </summary>
    private static void OutputHelp()
    {
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  --new --title=\"...\" --content=\"...\" --background=\"Yellow\"");
        Console.WriteLine("  --edit=ID --title=\"...\" --content=\"...\" --background=\"Yellow\"");
        Console.WriteLine("  --delete=ID [--confirm=\"true|false\"]");
        Console.WriteLine("  --get=ID");
        Console.WriteLine("  --list");
        Console.WriteLine();
        Console.WriteLine("Backgrounds:");
        Console.WriteLine(BackgroundModel.NamesOutput);
        Console.WriteLine();
    }

    /// <summary>
    /// Writes details of specified note to console
    /// </summary>
    /// <param name="note">The note to display</param>
    private static void OutputNote(NoteModel note)
    {
        Output(ConsoleColor.Green, $"Id: {note.Id}");
        Console.WriteLine($" Title     : {note.Title}");
        Console.WriteLine($" Content   : {note.Content}");
        Console.WriteLine($" Background: {GetBackgroundOutput(note.Background)}");
        Console.WriteLine($" URI       : {note.AssetDataUri}");
        Console.WriteLine();
    }

    /// <summary>
    /// Try Resolve Background
    /// </summary>
    /// <param name="input">Input</param>
    /// <param name="background">Background</param>
    /// <returns>True if Valid, otherwise False</returns>
    private static bool TryResolveBackground(string input, out string background)
    {
        background = string.Empty;
        if (string.IsNullOrWhiteSpace(input))
            return false;
        if (int.TryParse(input, out var index) && index >= 1 && index <= BackgroundModel.Options.Length)
        {
            background = BackgroundModel.Options[index - 1].Background;
            return true;
        }
        if (BackgroundModel.NameToBackground?.TryGetValue(input, out var fromName) == true)
        {
            background = fromName;
            return true;
        }
        if (BackgroundModel.BackgroundToName?.ContainsKey(input) == true)
        {
            background = input;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Prompt Required Value
    /// </summary>
    /// <param name="label">Value Label</param>
    /// <returns>Value</returns>
    /// <exception cref="OperationCanceledException">Input Cancelled</exception>
    private string PromptRequired(string label)
    {
        while (true)
        {
            Output(ConsoleColor.Cyan, $"{label}: ", inline: true);
            if (!TryReadLineAllowEsc(out var value))
            {
                if (_action == ConsoleAction.New)
                    throw new OperationCanceledException("Input cancelled by user.");
                Output(ConsoleColor.Red, $"{label} is required.");
                continue;
            }
            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();
            Output(ConsoleColor.Red, $"{label} is required.");
        }
    }

    /// <summary>
    /// Prompt Value with Default
    /// </summary>
    /// <param name="label">Label</param>
    /// <param name="current">Current</param>
    /// <param name="validateBackground">Should Validate Background</param>
    /// <returns>Value</returns>
    /// <exception cref="OperationCanceledException">Input Cancelled</exception>
    private static string? PromptWithDefault(string label, string current, bool validateBackground = false)
    {
        while (true)
        {
            var currentDisplay = label.Equals("Background", StringComparison.OrdinalIgnoreCase)
                ? GetBackgroundOutput(current) : current;
            if (validateBackground)
            {
                Console.WriteLine();
                Output(ConsoleColor.Cyan, "Available background colours:");
                for (int i = 0; i < BackgroundModel.Options.Length; i++)
                    Console.WriteLine($"  {i + 1}) {BackgroundModel.Options[i].Name}");
            }
            Output(ConsoleColor.Cyan, $"{label} [{currentDisplay}]: ", true);
            if (!TryReadLineAllowEsc(out var value))
                throw new OperationCanceledException("Input cancelled by user.");
            if (string.IsNullOrEmpty(value))
                return null; // Keep Existing
            value = value.Trim();
            if (validateBackground)
            {
                if (TryResolveBackground(value, out var background))
                    return background;
                Output(ConsoleColor.Red, $"Invalid background. Enter number, name, or one of: {BackgroundModel.NamesOutput}");
                continue;
            }
            return value;
        }
    }

    /// <summary>
    /// Prompt Background
    /// </summary>
    /// <returns>Value</returns>
    /// <exception cref="OperationCanceledException">Input Cancelled</exception>
    private static string PromptBackground()
    {
        while (true)
        {
            Console.WriteLine();
            Output(ConsoleColor.Cyan, "Available background colours:");
            for (int i = 0; i < BackgroundModel.Options.Length; i++)
                Console.WriteLine($"  {i + 1}) {BackgroundModel.Options[i].Name}");
            Output(ConsoleColor.Cyan, "Background (enter number or name): ", inline: true);
            if (!TryReadLineAllowEsc(out var value))
                throw new OperationCanceledException("Input cancelled by user.");
            var trimmed = value?.Trim() ?? string.Empty;
            if (TryResolveBackground(trimmed, out var background))
                return background;
            Output(ConsoleColor.Red, $"Invalid background. Enter number, name, or one of: {BackgroundModel.NamesOutput}");
        }
    }

    /// <summary>
    /// Ensure Id is provided and valid
    /// </summary>
    private void EnsureId()
    {
        while (string.IsNullOrWhiteSpace(_id) || TryParse(_id) is null)
        {
            var entered = PromptRequired("Id");
            if (TryParse(entered) is null)
            {
                Output(ConsoleColor.Red, "Id must be numeric.");
                continue;
            }
            _id = entered;
        }
    }

    /// <summary>
    /// Prompt for any missing values when creating a new note
    /// </summary>
    private void PromptMissingForNew()
    {
        if (string.IsNullOrWhiteSpace(_title))
            _title = PromptRequired("Title");
        if (string.IsNullOrWhiteSpace(_content))
            _content = PromptRequired("Content");
        if (string.IsNullOrWhiteSpace(_background))
            _background = PromptBackground();
    }

    /// <summary>
    /// Prompt for any missing values when editing an existing note
    /// </summary>
    /// <param name="existing">Note Model</param>
    private void PromptMissingForEdit(NoteModel existing)
    {
        EnsureId();
        if (string.IsNullOrWhiteSpace(_title))
            _title = PromptWithDefault("Title", existing.Title);
        if (string.IsNullOrWhiteSpace(_content))
            _content = PromptWithDefault("Content", existing.Content);
        if (string.IsNullOrWhiteSpace(_background))
            _background = PromptWithDefault("Background", existing.Background, validateBackground: true);
        _title ??= existing.Title;
        _content ??= existing.Content;
        _background ??= existing.Background;
    }

    /// <summary>
    /// Prompt for confirmation before deleting a note
    /// </summary>
    /// <returns>True if Confirm Delete, otherwise Cancel</returns>
    private static bool PromptConfirmForDelete()
    {
        while (true)
        {
            Console.WriteLine();
            Output(ConsoleColor.Cyan, "Are you sure you want to delete this note?");
            Console.WriteLine("  Y) Yes");
            Console.WriteLine("  N) No");
            Output(ConsoleColor.Cyan, "Enter choice: ", inline: true);
            var first = Console.ReadKey(true);
            if (first.Key == ConsoleKey.Escape)
            {
                Console.WriteLine();
                return false;
            }
            if (first.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                Output(ConsoleColor.Red, "Please choose Y or N");
                continue;
            }
            Console.Write(first.KeyChar);
            var rest = Console.ReadLine() ?? string.Empty;
            var input = (first.KeyChar + rest).Trim().ToLowerInvariant();
            switch (input)
            {
                case "y":
                case "yes":
                    return true;
                case "n":
                case "no":
                    return false;
            }
            Output(ConsoleColor.Red, "Invalid selection. Choose Y or N");
        }
    }

    /// <summary>
    /// Build Note Model from Inputs
    /// </summary>
    /// <returns>Note Model</returns>
    private NoteModel? BuildNoteModel()
    {
        var model = new NoteModel();
        if (_id is not null)
            model.Id = TryParse(_id);
        if (_title is not null)
            model.Title = _title;
        if (_content is not null)
            model.Content = _content;
        if (_background is not null)
            model.Background = _background;
        return model;
    }

    /// <summary>
    /// Handle New
    /// </summary>    
    private async Task HandleNew()
    {
        try
        {
            PromptMissingForNew();
        }
        catch (OperationCanceledException)
        {
            Output(ConsoleColor.Yellow, "New cancelled.");
            return;
        }
        var note = BuildNoteModel();
        if (note == null)
            return;
        var result = await _application.NewAsync(note, skipDialog: true);
        Output(result ? ConsoleColor.Green : ConsoleColor.Red,
            result ? "Note created." : "Failed to create note.");
    }

    /// <summary>
    /// Handle Edit
    /// </summary>    
    private async Task HandleEdit()
    {
        try
        {
            EnsureId();
            NoteModel? existing = null;
            if (TryParse(_id) is int idValue)
                existing = await _application.GetAsync(idValue);
            if (existing == null)
            {
                Output(ConsoleColor.Red, "Note not found.");
                return;
            }
            PromptMissingForEdit(existing);
            var model = new NoteModel
            {
                Id = existing.Id,
                Title = _title ?? existing.Title,
                Content = _content ?? existing.Content,
                Background = _background ?? existing.Background
            };
            var result = await _application.EditAsync(model, skipDialog: true);
            Output(result ? ConsoleColor.Green : ConsoleColor.Red,
                result ? "Note updated." : "Failed to update note.");
        }
        catch (OperationCanceledException)
        {
            Output(ConsoleColor.Yellow, "Edit cancelled.");
            return;
        }
    }

    /// <summary>
    /// Handle Delete
    /// </summary>    
    private async Task HandleDelete()
    {
        EnsureId();
        if (TryParse(_id) is not int idValue)
        {
            Output(ConsoleColor.Red, "Delete requires a valid numeric id.");
            return;
        }
        await _application.ListAsync();
        var note = _application.Content.Notes.FirstOrDefault(n => n.Id == idValue);
        if (note == null)
        {
            Output(ConsoleColor.Red, "Note not found.");
            return;
        }
        OutputNote(note);
        if (_confirm == null || _confirm == true)
        {
            if (!PromptConfirmForDelete())
            {
                Output(ConsoleColor.Yellow, "Delete cancelled.");
                return;
            }
        }
        var result = await _application.DeleteAsync(note, skipDialog: true);
        Output(result ? ConsoleColor.Green : ConsoleColor.Red,
            result ? "Note deleted." : "Failed to delete note.");
    }

    /// <summary>
    /// Handle Get
    /// </summary>    
    private async Task HandleGet()
    {
        EnsureId();
        if (TryParse(_id) is not int idValue)
        {
            Output(ConsoleColor.Red, "Get requires a valid numeric id.");
            return;
        }
        var note = await _application.GetAsync(idValue);
        if (note == null)
        {
            Output(ConsoleColor.Yellow, "Note not found.");
            return;
        }
        OutputNote(note);
    }

    /// <summary>
    /// Handle List
    /// </summary>    
    private async Task HandleList()
    {
        await _application.ListAsync();
        if (_application.Content.Notes.Count == 0)
        {
            Output(ConsoleColor.Yellow, "No notes found.");
            return;
        }
        foreach (var n in _application.Content.Notes.OrderBy(n => n.Id))
            OutputNote(n);
    }

    /// <summary>
    /// Execute Action
    /// </summary>
    /// <param name="action">Console Action</param>    
    private async Task ExecuteAsync(ConsoleAction action)
    {
        switch (action)
        {
            case ConsoleAction.New:
                await HandleNew();
                break;
            case ConsoleAction.Edit:
                await HandleEdit();
                break;
            case ConsoleAction.Delete:
                await HandleDelete();
                break;
            case ConsoleAction.Get:
                await HandleGet();
                break;
            case ConsoleAction.List:
                await HandleList();
                break;
            default:
                OutputHelp();
                break;
        }
    }

    /// <summary>
    /// Prompt for Action in interactive mode
    /// </summary>
    /// <returns>Action and Exit</returns>
    private static (ConsoleAction action, bool exit) PromptInteractiveAction()
    {
        while (true)
        {
            Console.WriteLine();
            Output(ConsoleColor.Cyan, "Select an action or ESC to Exit:");
            Console.WriteLine("  1) New");
            Console.WriteLine("  2) Edit");
            Console.WriteLine("  3) Delete");
            Console.WriteLine("  4) Get");
            Console.WriteLine("  5) List");
            Console.WriteLine("  0) Exit");
            Output(ConsoleColor.Cyan, "Enter number or name: ", inline: true);
            var first = Console.ReadKey(intercept: true);
            if (first.Key == ConsoleKey.Escape)
                return (ConsoleAction.None, true);
            if (first.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                Output(ConsoleColor.Red, "Please choose an option.");
                continue;
            }
            Console.Write(first.KeyChar);
            var rest = Console.ReadLine() ?? string.Empty;
            var input = (first.KeyChar + rest).Trim().ToLowerInvariant();
            switch (input)
            {
                case "0":
                case "q":
                case "quit":
                case "exit":
                    return (ConsoleAction.None, true);
                case "1":
                case "n":
                case "new":
                    return (ConsoleAction.New, false);
                case "2":
                case "e":
                case "edit":
                    return (ConsoleAction.Edit, false);
                case "3":
                case "d":
                case "del":
                case "delete":
                    return (ConsoleAction.Delete, false);
                case "4":
                case "g":
                case "get":
                    return (ConsoleAction.Get, false);
                case "5":
                case "l":
                case "list":
                    return (ConsoleAction.List, false);
            }
            Output(ConsoleColor.Red, "Invalid selection.");
        }
    }

    /// <summary>
    /// Reset Inputs
    /// </summary>
    private void ResetInputs()
    {
        _id = null;
        _title = null;
        _content = null;
        _background = null;
    }

    /// <summary>
    /// Check if action command and / or arguments are valid
    /// </summary>
    /// <param name="error">Error</param>
    /// <returns>True if Valid, otherwise False</returns>
    private bool IsCommandArgumentsValid(out string? error)
    {
        error = null;
        if (!_hasAction)
            return true;
        if (_hasConflict)
        {
            error = "Multiple commands specified. Use only one of: new, edit, delete, get, list.";
            return false;
        }
        switch (_action)
        {
            case ConsoleAction.New:
                if (!AllSpecified(_title, _content, _background))
                {
                    error = "New requires --title, --content and --background.";
                    return false;
                }
                if (!TryResolveBackground(_background!, out _background!))
                {
                    error = $"Background must be one of: {BackgroundModel.NamesOutput}";
                    return false;
                }
                break;
            case ConsoleAction.Edit:
                if (_id is null)
                {
                    error = "Edit requires an id.";
                    return false;
                }
                if (!AllSpecified(_title, _content, _background))
                {
                    error = "Edit requires --title, --content and --background.";
                    return false;
                }
                if (!TryResolveBackground(_background!, out _background!))
                {
                    error = $"Background must be one of: {BackgroundModel.NamesOutput}";
                    return false;
                }
                break;
            case ConsoleAction.Delete:
            case ConsoleAction.Get:
                if (_id is null)
                {
                    error = $"{_action} requires an id.";
                    return false;
                }
                break;
        }
        return true;
    }

    /// <summary>
    /// Interactive Mode
    /// </summary>    
    private async Task Interactive()
    {
        Output(ConsoleColor.Yellow, "ConsoleNotes - Interactive mode. Press ESC at Menu to Exit.");
        while (true)
        {
            var (action, exit) = PromptInteractiveAction();
            if (exit)
                break;
            if (action == ConsoleAction.None)
                continue;
            ResetInputs();
            _action = action;
            await ExecuteAsync(_action);
        }
    }

    /// <summary>
    /// Try Read Line and Allow Escape Key
    /// </summary>
    /// <param name="value">Input Value</param>
    /// <returns>True if Escape, otherwise False</returns>
    private static bool TryReadLineAllowEsc(out string value)
    {
        var sb = new StringBuilder();
        while (true)
        {
            var keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                Console.WriteLine();
                value = string.Empty;
                return false;
            }
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                value = sb.ToString();
                return true;
            }
            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (sb.Length > 0)
                {
                    sb.Length--;
                    Console.Write("\b \b");
                }
                continue;
            }
            var ch = keyInfo.KeyChar;
            if (!char.IsControl(ch))
            {
                sb.Append(ch);
                Console.Write(ch);
            }
        }
    }

    /// <summary>
    /// Populate
    /// </summary>
    /// <param name="arguments">Arguments</param>
    private void Populate(Dictionary<string, string> arguments)
    {
        var actions = new List<(ConsoleAction action, string?)>();
        if (arguments.Count > 0)
        {
            AddActionIfPresent(arguments, new_command, actions, ConsoleAction.New);
            AddActionIfPresent(arguments, edit_command, actions, ConsoleAction.Edit);
            AddActionIfPresent(arguments, delete_command, actions, ConsoleAction.Delete);
            AddActionIfPresent(arguments, get_command, actions, ConsoleAction.Get);
            AddActionIfPresent(arguments, list_command, actions, ConsoleAction.List);
            AddActionIfPresent(arguments, help_command, actions, ConsoleAction.Help);
            if (arguments.TryGetValue(id_argument, out var id)) _id = id;
            if (arguments.TryGetValue(title_argument, out var title)) _title = title;
            if (arguments.TryGetValue(content_argument, out var content)) _content = content;
            if (arguments.TryGetValue(background_argument, out var bg)) _background = bg;
            if (arguments.TryGetValue(confirm_argument, out var confirm) &&
                bool.TryParse(confirm, out var confirmValue))
                _confirm = confirmValue;
        }
        _hasConflict = actions.Select(a => a.action).Distinct().Count() > 1;
        if (actions.Count > 0)
        {
            _action = actions[0].action;
            _hasAction = true;
            foreach (var (action, value) in actions)
            {
                if (action is ConsoleAction.Edit or ConsoleAction.Delete or ConsoleAction.Get && 
                    TryParse(value) is int)
                {
                    _id = value;
                    break;
                }
            }
        }
        if (!string.IsNullOrWhiteSpace(_background) &&
            TryResolveBackground(_background, out var background))
            _background = background;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="arguments">Console Arguments Provider</param>
    /// <param name="application">Application Provider</param>
    public ConsoleProvider(ConsoleArgumentProvider arguments, IApplicationProvider application)
    {        
        _application = application;
        Populate(arguments.Arguments);
    }

    /// <summary>
    /// Run Application
    /// </summary>
    public async Task Run()
    {
        // Execute and Exit if Action provided via CLI
        if (_hasAction)
        {
            if (!IsCommandArgumentsValid(out var error))
            {
                Output(ConsoleColor.Red, error ?? "Invalid arguments.");
                OutputHelp();
                return;
            }
            await ExecuteAsync(_action);
            return;
        }
        // Otherwise Interactive mode until select Exit or Esc
        await Interactive();
    }    
}
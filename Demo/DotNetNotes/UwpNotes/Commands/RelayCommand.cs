namespace UwpNotes.Commands;

/// <summary>
/// Relay Command
/// </summary>
/// <param name="command">Command</param>
public partial class RelayCommand(ICommand command) : ICommand
{
    /// <summary>
    /// Can Execute Changed Event Handler
    /// </summary>

    public event EventHandler? CanExecuteChanged
    {
        add => command.CanExecuteChanged += value;
        remove => command.CanExecuteChanged -= value;
    }

    /// <summary>
    /// Can Execute?
    /// </summary>
    /// <param name="parameter">Object Parameter</param>
    /// <returns>True if can Execute, False if Not</returns>
    public bool CanExecute(object? parameter) =>
        command.CanExecute(parameter);

    /// <summary>
    /// Execute and raise Executed event.
    /// </summary>
    /// <param name="parameter">Object Parameter</param>
    public void Execute(object? parameter)
    {
        command.Execute(parameter);
        OnExecuted();
    }

    /// <summary>
    /// Raised after execution.
    /// </summary>
    public event EventHandler? Executed;

    /// <summary>
    /// On Executed
    /// </summary>
    protected virtual void OnExecuted() => 
        Executed?.Invoke(this, EventArgs.Empty);
}
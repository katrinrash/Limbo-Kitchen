/// <summary>
/// Defines executable behavior used by the state machine. Any class implementing this interface can be executed as part of a gameplay state.
/// </summary>
///
/// Architecture:
/// Uses an interface-based approach to keep state logic modular, reusable and loosely coupled.

public interface IExecutable
{
    /// <summary>
    /// Shared handler used for communication between executable logic and the state machine.
    /// </summary>
    public IStateHandler StateHandler { get; set; }

    #region Functions

    /// <summary>
    /// Starts execution logic for the current state.
    /// </summary>
    public void Execute();

    /// <summary>
    /// Stops execution logic and performs cleanup if needed.
    /// </summary>
    public void EndExecution();

    /// <summary>
    /// Marks current execution step as completed and informs the state handler.
    /// </summary>
    ///
    /// Note:
    /// Default interface implementation is used here to avoid duplicated completion logic across states.
    public void SetAsCompleted()
    {
        StateHandler?.CompleteStep();
    }

    #endregion
}
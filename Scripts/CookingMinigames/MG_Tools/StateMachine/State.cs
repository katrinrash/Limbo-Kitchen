/// <summary>
/// Represents a single state inside the state machine. Responsible for executing and stopping logic associated with a specific gameplay state.
/// </summary>
///
/// Lifecycle:
/// Enter()  -> Starts execution
/// Exit()   -> Stops execution

public class State
{
    // Object responsible for the actual state behavior
    private readonly IExecutable _executable;

    /// <summary>
    /// Unique identifier used by the state machine.
    /// </summary>
    public int ID { get; private set; }

    /// <summary>
    /// Creates a new state instance.
    /// </summary>

    public State(IExecutable executable, int id, IStateHandler stateHandler)
    {
        // Store reference to the executable logic
        _executable = executable;

        // Inject shared state handler into executable logic
        _executable.StateHandler = stateHandler;

        // Assign unique ID for the state
        ID = id;
    }

    /// <summary>
    /// Activates the state and starts its execution logic.
    /// </summary>
    public virtual void Enter()
    {
        _executable.Execute();
    }

    /// <summary>
    /// Deactivates the state and stops its execution logic.
    /// </summary>
    public virtual void Exit()
    {
        _executable.EndExecution();
    }
}
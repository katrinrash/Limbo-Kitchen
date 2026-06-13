
// Represents a single state inside the state machine. Responsible for executing and stopping logic associated with a specific gameplay state.
public class State
{
    public int ID { get; private set; }

    private readonly IExecutable _executable;

    public State(IExecutable executable, int id, IStateHandler stateHandler)
    {
        _executable = executable;

        _executable.StateHandler = stateHandler;

        ID = id;
    }

    public virtual void Enter()
    {
        _executable.Execute();
    }

    public virtual void Exit()
    {
        _executable.EndExecution();
    }
}
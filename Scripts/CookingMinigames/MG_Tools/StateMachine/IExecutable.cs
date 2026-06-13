
// Defines executable behavior used by the state machine
public interface IExecutable
{
    public IStateHandler StateHandler { get; set; }

    #region Functions

    public void Execute();

    public void EndExecution();

    public void SetAsCompleted()
    {
        StateHandler?.CompleteStep();
    }

    #endregion
}
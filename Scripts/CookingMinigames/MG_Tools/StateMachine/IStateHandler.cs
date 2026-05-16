/// <summary>
/// Provides a communication bridge between state execution logic and external systems (such as a state machine).
/// </summary>
///
/// Architecture:
/// This interface is intentionally minimal to keep the system loosely coupled and easily extendable.

public interface IStateHandler
{
    /// <summary>
    /// Called when a state or execution step is completed. Used to trigger next transitions or progress game flow.
    /// </summary>
    
    public void CompleteStep();
}
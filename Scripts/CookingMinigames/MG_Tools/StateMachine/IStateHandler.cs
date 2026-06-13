
// Provides a communication bridge between state execution logic and external systems (such as a state machine)
public interface IStateHandler
{
    public void CompleteStep();
}
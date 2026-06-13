using System;
using System.Collections.Generic;

// Base state machine implementation responsible for storing states and handling transitions between them.
// Each state is identified by a unique ID and stored inside a dictionary for fast access.

public class BaseStateMachine
{
    private State _currentState;
    private Dictionary<int, State> _states = new Dictionary<int, State>();

    public void AddState(State state)
    {
        _states.Add(state.ID, state);
    }

    public void SetState(State state = null)
    {
        int id = state?.ID ?? -1;

        _states.TryGetValue(id, out var newState);

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }
}
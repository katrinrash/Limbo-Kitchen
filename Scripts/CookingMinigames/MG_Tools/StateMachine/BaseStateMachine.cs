using System;
using System.Collections.Generic;

/// <summary>
/// Base state machine implementation responsible for storing states and handling transitions between them.
/// Each state is identified by a unique ID and stored inside a dictionary for fast access.
/// </summary>
///
/// State transition flow:
/// 1. Exit current state
/// 2. Assign new state
/// 3. Enter new state

public class BaseStateMachine
{
    // Currently active state
    private State _currentState;

    // Collection of all available states indexed by ID
    private Dictionary<int, State> _states = new Dictionary<int, State>();

    /// <summary>
    /// Registers a new state inside the state machine.
    /// </summary>

    public void AddState(State state)
    {
        _states.Add(state.ID, state);
    }

    /// <summary>
    /// Changes the currently active state.
    /// </summary>

    public void SetState(State state = null)
    {
        // Use -1 as fallback ID when state is null
        int id = state?.ID ?? -1;

        // Attempt to retrieve target state from collection
        _states.TryGetValue(id, out var newState);

        // Exit previous state before switching if it exists
        _currentState?.Exit();

        // Assign new active state or null if it doesn't exist 
        _currentState = newState;

        // Initialize new state if it exists
        _currentState?.Enter();
    }
}
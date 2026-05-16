using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main controller for a staged minigame flow. Manages state creation, progression, and completion logic.
/// </summary>
///
/// Purpose:
/// - Controls sequential execution of minigame stages
/// - Acts as a bridge between state machine and gameplay systems
/// - Handles progression, skipping (tutorial), and completion
///
/// Flow:
/// StartMinigame → State 0 → CompleteStep → next state → ... → End

public class BaseMinigameManager : MonoBehaviour, IStateHandler
{
    // List of executables (gameplay logic components) assigned in Inspector
    [SerializeField] private List<MonoBehaviour> executables;

    // Number of stages in the minigame, set in Inspector
    [SerializeField] private int _stages;

    // Core state machine responsible for transitions
    private BaseStateMachine _stateMachine;

    // All states representing minigame steps
    private List<State> _states;

    // Tracks current stage index in progression
    private int _stateIndex;

    private void Start()
    {
        // Initialize state machine and states
        _stateMachine = new BaseStateMachine();
        _states = new List<State>();

        // Create all states based on assigned executables
        CreateAllStates();

        // Register states in the state machine
        foreach (var state in _states)
        {
            _stateMachine.AddState(state);
        }
    }

    /// <summary>
    /// Creates all state instances from assigned executables. Each executable represents one stage of the minigame.
    /// </summary>

    private void CreateAllStates()
    {
        for (int i = 0; i < _stages; i++)
        {
            // Convert MonoBehaviour to IExecutable so it can be used inside the state system
            _states.Add(new State((IExecutable)executables[i], i, this));

            // Ensure all stages are inactive by default at the beginning
            executables[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Initializes and starts the minigame sequence. Resets progression and activates the first state.
    /// </summary>

    public void StartMinigame()
    {
        // Reset progression index
        _stateIndex = 0;

        // Enter first stage
        _stateMachine.SetState(_states[_stateIndex++]);

        // Connect external UI button to state machine control
        StopMGButton.Instance.SetStateMachine(_stateMachine);
    }

    /// <summary>
    /// Called by state system when a stage is completed. Moves to the next stage or ends the minigame.
    /// </summary>
    
    public void CompleteStep()
    {
        // Check if all stages are finished and end minigame if so
        if (_stateIndex >= _states.Count)
        {
            // Clear current state
            _stateMachine.SetState(); 

            // Notify global manager that minigame is complete
            MinigamesManager.Instance.EndMinigame(true);

            return;
        }

        // Move to next stage
        _stateMachine.SetState(_states[_stateIndex++]);
    }

    /// <summary>
    /// Skips directly to a specific stage (used for tutorial).
    /// </summary>
    /// 
    /// Originally implemented by another developer - Julia Budzisz, who is responsible for the tutorial system.

    public void SkipToState(int index)
    {
        if (index >= 0 && index < _states.Count)
        {
            foreach (var e in executables)
                e.gameObject.SetActive(false);

            _stateMachine.SetState();

            _stateIndex = index;
            _stateMachine.SetState(_states[_stateIndex++]);

            Debug.Log($"[Tutorial] Switched to step: {index}");
        }
    }
}

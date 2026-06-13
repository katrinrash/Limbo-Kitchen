using System.Collections.Generic;
using UnityEngine;

// Main controller for a staged minigame flow. Manages state creation, progression, and completion logic.
//
// Purpose:
// - Controls sequential execution of minigame stages
// - Acts as a bridge between state machine and gameplay systems
// - Handles progression, skipping (tutorial), and completion

public class BaseMinigameManager : MonoBehaviour, IStateHandler
{
    [SerializeField] private List<MonoBehaviour> executables;
    [SerializeField] private int _stages;

    private BaseStateMachine _stateMachine;
    private List<State> _states;
    private int _stateIndex;

    private void Start()
    {
        _stateMachine = new BaseStateMachine();
        _states = new List<State>();

        CreateAllStates();

        foreach (var state in _states)
        {
            _stateMachine.AddState(state);
        }
    }

    private void CreateAllStates()
    {
        for (int i = 0; i < _stages; i++)
        {
            _states.Add(new State((IExecutable)executables[i], i, this));

            executables[i].gameObject.SetActive(false);
        }
    }

    public void StartMinigame()
    {
        _stateIndex = 0;
        _stateMachine.SetState(_states[_stateIndex++]);

        StopMGButton.Instance.SetStateMachine(_stateMachine);
    }
    
    public void CompleteStep()
    {
        if (_stateIndex >= _states.Count)
        {
            _stateMachine.SetState(); 

            MinigamesManager.Instance.EndMinigame(true);

            return;
        }

        _stateMachine.SetState(_states[_stateIndex++]);
    }

    // Skips directly to a specific stage (used for tutorial).
    // Originally implemented by another developer - Julia Budzisz, who is responsible for the tutorial system.

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

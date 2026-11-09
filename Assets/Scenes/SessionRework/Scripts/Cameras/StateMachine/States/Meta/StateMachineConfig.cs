using System.Collections.Generic;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;
using UnityEngine;

[CreateAssetMenu(fileName = "StateMachineConfig", menuName = "Camera/StateMachine/MachineConfig")]
public class StateMachineConfig : ScriptableObject
{
    [SerializeField]
    private List<CameraStateConfig> _states = new();

    [SerializeField]
    private CameraStateConfig _initialState;
    
    [SerializeField]
    private List<CameraStateTransitionConfig> _transitions = new();
    
    public IReadOnlyList<CameraStateConfig> States => _states;
    public CameraStateConfig InitialState => _initialState;
    public IReadOnlyList<CameraStateTransitionConfig> Transitions => _transitions;
    
    
    
    public bool ContainsState(CameraStateConfig state)
    {
        return state != null && _states.Contains(state);
    }

    public bool CanTransition(
        CameraStateConfig from,
        CameraStateConfig to)
    {
        if (from == null || to == null)
            return false;

        foreach (CameraStateTransitionConfig transition in _transitions)
        {
            if (transition.From == from && transition.To == to)
                return true;
        }

        return false;
    }

    private void OnValidate()
    {
        if (_initialState != null && !_states.Contains(_initialState))
        {
            Debug.LogWarning(
                $"Начальное состояние '{_initialState.name}' " +
                $"отсутствует в списке States машины '{name}'.",
                this);
        }
    }
}

using UnityEngine;
using Scenes.SessionRework.Scripts.Cameras.StateMachine;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.Interfaces;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Zenject;

using System;
using System.Collections.Generic;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;
using UnityEngine;
using Zenject;

public class CameraStateMachine : ICameraStateMachine
{
    private readonly StateMachineConfig _machineConfig;
    private readonly IStateFactory _stateFactory;

    private readonly Dictionary<CameraStateConfig, IState> _states = new();
    private readonly Dictionary<Type, IState> _existsStates = new();
    private readonly Dictionary<Type, CameraStateConfig> _stateConfigs = new();

    private IState _currentState;
    private CameraStateConfig _currentStateConfig;

    public IState CurrentState => _currentState;

    [Inject]
    public CameraStateMachine(
        StateMachineConfig machineConfig,
        IStateFactory stateFactory)
    {
        _machineConfig = machineConfig;
        _stateFactory = stateFactory;
    }

    public void Initialize()
    {
        if (_machineConfig.InitialState == null)
        {
            Debug.LogError("Не задано начальное состояние камеры.");
            return;
        }

        if (!_machineConfig.ContainsState(_machineConfig.InitialState))
        {
            Debug.LogError(
                "Начальное состояние отсутствует в списке состояний машины.");
            return;
        }

        foreach (CameraStateConfig stateConfig in _machineConfig.States)
        {
            if (stateConfig == null)
            {
                Debug.LogWarning(
                    "В конфигурации машины обнаружено пустое состояние.");
                continue;
            }

            if (_states.ContainsKey(stateConfig))
            {
                Debug.LogWarning(
                    $"Состояние '{stateConfig.name}' указано несколько раз.");
                continue;
            }

            var state = _stateFactory.CreateState(stateConfig);

            Type stateType = state.GetType();

            if (_existsStates.ContainsKey(stateType))
            {
                Debug.LogWarning(
                    $"Состояние типа '{stateType.Name}' уже зарегистрировано.");

                continue;
            }
            
            _stateConfigs.Add(stateType, stateConfig);
            _existsStates.Add(state.GetType(),state);
            _states.Add(
                stateConfig,
                state
                );
        }

        ChangeState(_machineConfig.InitialState, force: true);
    }

    public void Update(float deltaTime)
    {
        _currentState?.Update(deltaTime);
    }

    public bool ChangeState<T>() where T : IState
    {
        Type stateType = typeof(T);

        if (!_existsStates.TryGetValue(stateType, out IState nextState))
        {
            Debug.LogWarning(
                $"Состояние типа '{stateType.Name}' не зарегистрировано.");

            return false;
        }

        if (!_stateConfigs.TryGetValue(
                stateType, out CameraStateConfig nextStateConfig))
        {
            Debug.LogWarning(
                $"Конфигурация состояния '{stateType.Name}' не найдена.");

            return false;
        }

        return ChangeState(nextStateConfig, force: false);
    }

    public bool ChangeState(Type stateType)
    {
        if (!_existsStates.TryGetValue(stateType, out IState nextState))
        {
            Debug.LogWarning(
                $"Состояние типа '{stateType.Name}' не зарегистрировано.");

            return false;
        }
        
        if (!_stateConfigs.TryGetValue(
                stateType, out CameraStateConfig nextStateConfig))
        {
            Debug.LogWarning(
                $"Конфигурация состояния '{stateType.Name}' не найдена.");

            return false;
        }

        return ChangeState(nextStateConfig, force: false);
    }

    private bool ChangeState(
        CameraStateConfig nextStateConfig,
        bool force)
    {
        if (nextStateConfig == null)
        {
            Debug.LogWarning("Нельзя переключиться в пустое состояние.");
            return false;
        }

        if (!_states.TryGetValue(nextStateConfig, out IState nextState))
        {
            Debug.LogWarning(
                $"Состояние '{nextStateConfig.name}' " +
                "не зарегистрировано в машине состояний.");
            return false;
        }

        if (_currentStateConfig == nextStateConfig)
            return false;

        // При обычном переходе проверяем конфигурацию.
        if (!force &&
            !_machineConfig.CanTransition(
                _currentStateConfig,
                nextStateConfig))
        {
            Debug.LogWarning(
                $"Переход '{_currentStateConfig?.name}' -> " +
                $"'{nextStateConfig.name}' запрещён конфигурацией.");

            return false;
        }

        // Завершаем предыдущее состояние.
        if (_currentState is ICameraStateLifecycle previousLifecycle)
            previousLifecycle.Exit();

        _currentState = nextState;
        _currentStateConfig = nextStateConfig;

        // Активируем новое состояние.
        if (_currentState is ICameraStateLifecycle nextLifecycle)
            nextLifecycle.Enter();

        return true;
    }
}

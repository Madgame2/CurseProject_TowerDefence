using Common.Services.SceneServices;
using Common.systems.GameStates.States;
using System;
using UnityEngine;
using Zenject;

public class GameSessionState : BaseState
{
    [Inject] private SceneStateManager sceneStateManager;

    protected override void OnEnterToState(Type oldState)
    {
        this.sceneStateManager.loadScene<GameSessionScene>();
    }
}

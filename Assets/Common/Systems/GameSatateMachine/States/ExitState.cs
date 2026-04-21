using Common.systems.GameStates.States;
using UnityEngine;
using System;

public class ExitState : BaseState
{

    protected override void OnEnterToState(Type newState)
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}

using Common.systems.GameStates.States;
using UnityEngine;

public class ExitState : BaseState
{

    protected override void OnEnterToState()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}

using Common.systems.GameStates.States;
using System;
using UnityEngine;

public class SessionReworkState : BaseState
{

    protected override void OnEnterToState(Type oldState)
    {
        Debug.Log("SessionReworkState ENTER");
    }

    protected override void OnLeavFromState(Type newState)
    {
        
    }
}

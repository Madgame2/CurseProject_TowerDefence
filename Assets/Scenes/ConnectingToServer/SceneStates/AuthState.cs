using Common.Services.SceneServices.Scenes;
using Common.systems.SceneStates.States;
using Common.systems.SceneStates.States.Attributes;
using UnityEngine;

[RootState]
[LinkToScene(typeof(ConnectingToServerScene))]
public class AuthState : BaseState
{
    public override void EnterToState()
    {
        Debug.Log("Enter to auth state");
    }
}

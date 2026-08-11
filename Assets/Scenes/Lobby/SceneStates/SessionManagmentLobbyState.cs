using Common.systems.SceneStates.States;
using Common.systems.SceneStates.States.Attributes;
using UnityEngine;

[LinkToScene(typeof(LobbyScene))]
public class SessionManagementLobbyState : BaseState
{
    private readonly NavController _navController;

    public SessionManagementLobbyState(NavController navController)
    {
        _navController = navController;
    }

    public override void EnterToState()
    {
        _navController.ExecAnim("LobbyPage");
    }

    public override void LeaveFormState()
    {
        _navController.ExecReverseAnim("LobbyPage");
    }
}

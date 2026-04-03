using Common.systems.SceneStates.States;
using Common.systems.SceneStates.States.Attributes;
using UnityEngine;

[LinkToScene(typeof(LobbyScene))]
public class SettingsLobbyState:BaseState
{
    private readonly NavController _navController;

    public SettingsLobbyState(NavController navController)
    {
        _navController = navController;
    }

    public override void EnterToState()
    {
        _navController.ExecAnim("Settings");
    }

    public override void LeavFormState()
    {
        _navController.ExecReverseAnim("Settings");
    }
}

using Common.Services.SceneServices;
using Common.Services.SceneServices.Scenes;
using Common.systems.GameStates.States;
using Zenject;

public class LobbyState : BaseState
{
    private readonly SceneStateManager _sceneStateMannager;

    [Inject]
    public LobbyState(SceneStateManager sceneManager)
    {
        _sceneStateMannager = sceneManager;
    }
    protected override void OnEnterToState()
    {
        _sceneStateMannager.loadScene<LobbyScene>();
    }
}

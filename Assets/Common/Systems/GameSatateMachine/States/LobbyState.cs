using Common.Services.SceneServices;
using Common.Services.SceneServices.Scenes;
using Common.systems.GameStates.States;
using Common.systems.ProfileSystem;
using Scenes.Lobby;
using System.Threading.Tasks;
using Zenject;

public class LobbyState : BaseState
{
    private readonly SceneStateManager _sceneStateManager;


    [Inject]
    public LobbyState(SceneStateManager sceneManager)
    {
        _sceneStateManager = sceneManager;
    }
    protected override void OnEnterToState()
    {
        _sceneStateManager.loadScene<LobbyScene>();

    }
}

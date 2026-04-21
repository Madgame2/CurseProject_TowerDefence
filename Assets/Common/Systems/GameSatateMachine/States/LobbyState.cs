using Common.Services.SceneServices;
using Common.Services.SceneServices.Scenes;
using Common.systems.GameStates.States;
using Common.systems.ProfileSystem;
using Scenes.Lobby;
using System.Threading.Tasks;
using Zenject;
using System;
using Common.systems.MainThread;
using Common.systems.UI;
using System.Diagnostics;

public class LobbyState : BaseState
{
    private readonly SceneStateManager _sceneStateManager;
    private readonly MainThreadDispatcher _threadDispatcher;

    [Inject]
    public LobbyState(SceneStateManager sceneManager, MainThreadDispatcher mainThreadDispatcher)
    {
        _sceneStateManager = sceneManager;
        _threadDispatcher = mainThreadDispatcher;
    }
    protected override void OnEnterToState(Type newState)
    {
        _sceneStateManager.loadScene<LobbyScene>();

    }

    protected override void OnLeavFromState(Type newState)
    {
        if(newState == typeof(GameSessionState))
        {

            _threadDispatcher.Run(() =>
            {



            });
        }
    }
}

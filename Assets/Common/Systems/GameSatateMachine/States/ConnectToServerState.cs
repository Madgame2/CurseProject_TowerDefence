using Common.Services.SceneServices;
using Common.Services.SceneServices.Scenes;
using Common.systems.GameStates.States.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Common.systems.GameStates.States
{
    [RootState]
    public class ConnectToServerState : BaseState
    {
        private readonly SceneStateManager _sceneStateMannager;

        [Inject]
        public ConnectToServerState(SceneStateManager sceneManager)
        {
            _sceneStateMannager = sceneManager;
        }
        protected override void OnEnterToState()
        {
            _sceneStateMannager.loadScene<ConnectingToServerScene>();
            Debug.Log("enter to scene");
        }
    }
}

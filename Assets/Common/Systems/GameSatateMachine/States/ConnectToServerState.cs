using Common.Services.SceneServices;
using Common.Services.SceneServices.Scenes;
using Common.systems.GameStates.States.Attributes;
using Common.systems.ScriptDirectorSystem;
using Common.systems.ScriptDirectorSystem.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Common.systems.GameStates.States
{
    [RootState]
    public class ConnectToServerState : BaseState
    {
        private readonly SceneStateManager _sceneStateMannager;
        private readonly ScriptDirector scriptDirector;

        [Inject]
        public ConnectToServerState(SceneStateManager sceneManager,ScriptDirector director)
        {
            _sceneStateMannager = sceneManager;
            this.scriptDirector = director;
        }
        protected override void OnEnterToState()
        {
            _sceneStateMannager.loadScene<ConnectingToServerScene>();
            Debug.Log("enter to scene");
            scriptDirector.PlayScript<ConnectionToServerScript>();
        }
    }
}

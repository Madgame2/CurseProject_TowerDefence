using Common.Services.Net;
using Common.Services.SceneServices.Scenes;
using Common.systems.SceneStates;
using UnityEngine;

namespace Scenes.ConnectingToServerSceneModules
{
    public class ConnectingToServerSceneManager
    {
        private readonly NetService _hetService;
        private SceneStateMachine<ConnectingToServerScene> _sceneStateMachine;

        public ConnectingToServerSceneManager(NetService hetService, SceneStateMachine<ConnectingToServerScene> stateMachine)
        {
            _hetService = hetService;
            _sceneStateMachine = stateMachine;

            _hetService.tryingConnectingSelf += setTryingConnectionToServerState;
        }

        private void setTryingConnectionToServerState()
        {
            _sceneStateMachine.tryMoveToState(typeof(ConnectingState));
        }
    }
}

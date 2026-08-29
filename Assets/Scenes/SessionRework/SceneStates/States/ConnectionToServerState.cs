using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Services.Net.Interfaces;
using Common.Services.Net.Modules;
using Common.systems.SceneStates;
using Common.systems.SceneStates.States;
using Common.systems.SceneStates.States.Attributes;
using Scenes.SessionRework.Scripts.Network.Ineterfaces;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.SceneStates.States
{
    [RootState]
    [LinkToScene(typeof(SessionReworkScene))]
    public class ConnectionToServerState: BaseState
    {
        [Inject] private readonly INetworkClient  _networkClient;
        [Inject] private readonly ISessionSyncController _sessionSyncController;
        
        public override async void EnterToState()
        {
            try
            {
                await TryConnectToServer();
                
                _sessionSyncController.SubscribeToServerEvents();
                _sessionSyncController.SendClientReady();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        public override void LeaveFormState()
        {
            Debug.Log("LeaveFormState");
        }

        private async Task TryConnectToServer()
        {
            var headers = new Dictionary<string, string>() { { "EnterMode", "Development" }, };
            await _networkClient.TryCreateConnectionTo("127.0.0.1", 5041, headers);
        }
    }
}

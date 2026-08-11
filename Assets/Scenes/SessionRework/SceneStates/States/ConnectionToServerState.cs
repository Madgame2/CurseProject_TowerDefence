using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        [Inject] private readonly WebSocketModule _socket;
        [Inject]private readonly ISessionSyncController _sessionSyncController;
        
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
            var socket = await WebSocketModule.tryCreateConnectionTo("localhost:5041", headers);
            
            await _socket.ReplaceSocketAsync(socket); 
        }
    }
}

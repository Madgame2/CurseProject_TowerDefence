using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.systems.ProfileSystem;
using Common.systems.SceneStates;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SessionNetInstaller
{
    private WebSocketModule _socket;
    [Inject] private ProfileManager _profileManager;
    [Inject] private SceneStateMachine<GameSessionScene> _sceneStateMachine;
    [Inject] private ChankSystem _chankSystem;
    [Inject] private NetDispatcher _netDispatcher;

    private string _currentState;
    private float _currentProgress;

    public event Action<string> onStageUpdate;
    public event Action<float> onProgressUpdate;


    public string Stage { get => _currentState;
    private set {
            _currentState = value;
            onStageUpdate?.Invoke(value);
        }
    }
    public float CurrentProgress { 
        get => _currentProgress;
        private set {
            _currentProgress = value;
            onProgressUpdate?.Invoke(value);
        }
    }

    public SessionNetInstaller(NetService net)
    {
        _socket = net._webSocketModule;
    }

    public async Task Install(SessionServerInfo sessionInfo)
    {
        Stage = "Connection to server";
        CurrentProgress = 0.10f;
        var headers = new Dictionary<string, string>
                    {
                        { "Authorization", $"{sessionInfo.passToken}" },
                        { "X-User-Id", _profileManager.Profile.UserId },
                        { "x-session-id", sessionInfo.sessionId }
                    };

        ClientWebSocket newSocket = await WebSocketModule.CreateConnectionTo(sessionInfo.host, sessionInfo.port, headers);
        await _socket.ReplaceSessionSocketAsync(newSocket);


        Stage = "Waiting other players";
        CurrentProgress = 0.15f;

        _socket.On("sessionInstructions", SessionInstractionsHandler);
        _socket.On("awaitOtherCleints", waithOtherPlayershandler);
        _socket.On("startSession", HandlerStartSesion);
    }

    private async Task HandlerStartSesion(string arg)
    {

        _sceneStateMachine.tryMoveToState(typeof(ProcessSessionState));
    }

    private async Task waithOtherPlayershandler(string arg)
    {
        Stage = "Awaiting other players";
    }

    private Task UnpackMetaData()
    {
        var tcs = new TaskCompletionSource<bool>();

        void FinishedSync(string msg)
        {

            Unsubscribe();
            tcs.TrySetResult(true);
        }

        void Subscribe()
        {
            _chankSystem.InitSubscrationsToEvents();
            _socket.On("playerSyncFinished", FinishedSync);
        }

        void Unsubscribe()
        {
            _socket.Off("playerSyncFinished", FinishedSync);
        }

        

        Subscribe();


        return tcs.Task;
    }

    private async Task onPrealodedChunk(string arg)
    {
        Debug.Log(arg);
        _ = _socket.Send("chankApply", new { });
    }

    private void SessionInstractionsHandler(string arg)
    {
        Stage = "Obtaining metadata";
        CurrentProgress = 0.20f;

        _ = Task.Run(async () =>
        {
            await _socket.Send("toSyncReady", new { });

            await UnpackMetaData();


            Stage = "Obtaining metadata";
            CurrentProgress = 0.80f;
        });
    }

    public void ClearAll()
    {

    }
}

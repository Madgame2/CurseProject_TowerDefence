using Common.Exceptions.Net;
using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.Services.Net.Services;
using Common.Services.Tokens;
using Common.systems.GameStates;
using Common.systems.GameStates.States;
using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class EndGameViewModel
{
    [Inject] private GameStateMachine gameStateMachine;
    [Inject] private WebSocketModule _socket;
    [Inject] private NetService _netService;

    public Action<int> onWaveSelected;

    public void SetWaveNum(int waveNum)
    {
        onWaveSelected?.Invoke(waveNum);
    }


    public void onExitGameHandler()
    {
        this.gameStateMachine.tryMoveToState(typeof(ExitState));
    }

    internal async void onToLobby()
    {
        var accessToken = TokenManager.AccessToken;
        using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(20)))
        {

            CancellationToken ct = cts.Token;

            try
            {
                ClientWebSocket LobbyServerConnectin = await WebSocketModule.tryCreateConnectionTo("DESKTOP-JVVQE2J", 3000, ct, accessToken);
                await _socket.ReplaceSessionSocketAsync(LobbyServerConnectin);
                gameStateMachine.tryMoveToState(typeof(LobbyState));
            }
            catch (InvalidTokenException)
            {
                var authService = _netService.CreateAuthService();
                var response = await authService.tryUseRefrashToken(TokenManager.RefreshToken);

                switch (response.StatusCode)
                {
                    case 200:
                        await OnRefreshSuccess(response);
                        break;

                    case 400:
                        await OnRefreshBadRequest(response);
                        break;

                    case 401:
                        await OnRefreshUnauthorized();
                        break;

                    case 404:
                        await OnRefreshNotFound();
                        break;

                    case 500:
                        await OnRefreshServerError();
                        break;

                    default:
                        await OnRefreshUnknown(response);
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);

                await _socket.Disconnect();
                gameStateMachine.tryMoveToState(typeof(ConnectToServerState));
            }
        }
    }



    private async Task OnRefreshUnknown(HttpResponse response)
    {
        await _socket.Disconnect();
        gameStateMachine.tryMoveToState(typeof(ConnectToServerState));
    }

    private async Task OnRefreshServerError()
    {
        await _socket.Disconnect();
        gameStateMachine.tryMoveToState(typeof(ConnectToServerState));
    }

    private async Task OnRefreshNotFound()
    {
        await _socket.Disconnect();
        gameStateMachine.tryMoveToState(typeof(ConnectToServerState));
    }

    private async Task OnRefreshUnauthorized()
    {
        await _socket.Disconnect();
        gameStateMachine.tryMoveToState(typeof(ConnectToServerState));
    }

    private async Task OnRefreshBadRequest(HttpResponse response)
    {
        await _socket.Disconnect();
        gameStateMachine.tryMoveToState(typeof(ConnectToServerState));
    }

    private async Task OnRefreshSuccess(HttpResponse response)
    {
        AuthResponseDto dto = JsonConvert.DeserializeObject<AuthResponseDto>(response.Body);
        TokenManager.SetTokens(dto.accessToken, dto.refreshToken);

        try
        {
            using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(20)))
            {
                CancellationToken ct = cts.Token;

                ClientWebSocket LobbyServerConnectin = await WebSocketModule.tryCreateConnectionTo("DESKTOP-JVVQE2J", 3000, ct, dto.accessToken);
                await _socket.ReplaceSessionSocketAsync(LobbyServerConnectin);
                gameStateMachine.tryMoveToState(typeof(LobbyState));
            }

        }
        catch (Exception ex)
        {
            await _socket.Disconnect();
            gameStateMachine.tryMoveToState(typeof(ConnectToServerState));
        }
    }
}

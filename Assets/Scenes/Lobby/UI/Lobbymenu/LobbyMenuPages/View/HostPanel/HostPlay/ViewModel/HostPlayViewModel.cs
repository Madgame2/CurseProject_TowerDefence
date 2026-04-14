using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.systems.UI;
using Scenes.Lobby;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class HostPlayViewModel
{
    private readonly WebSocketModule _socket;
    private readonly LobbyManager _lobbyManager;
    private readonly UIManager _uiManager;
    private readonly System.Random _random = new System.Random();

    public event Action onLobbyChanges;

    public HostPlayViewModel(UIManager uiManager, NetService net, LobbyManager lobby)
    {
        _uiManager = uiManager;
        _socket = net._webSocketModule;
        _lobbyManager = lobby;
    }

    internal void OnBack()
    {
        _uiManager.Close("HostPlay");
    }

    public string Generate16DigitNumber()
    {
        StringBuilder sb = new StringBuilder(16);

        for (int i = 0; i < 16; i++)
        {
            int digit = _random.Next(0, 10); 
            sb.Append(digit);
        }

        return sb.ToString();
    }

    internal async void StartSearch(MatchMakingRequestDTO dto)
    {
        dto.LobbyId = _lobbyManager.Lobby.Id;

        using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(25)))
        {
            CancellationToken token = cts.Token;

            try
            {
                WSResponse res = await _socket.SendRequest("StartSession", dto, token);
                await HandleResponce(res);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("StartSession timeout → syncing state...");

                _lobbyManager.SyncState();
            }
        } 
        


        async Task HandleResponce(WSResponse res)
        {
            switch (res.Code)
            {
                case 200:

                    _lobbyManager.InGameSearch = true;
                    _uiManager.Close("HostPlay");

                    break;
            }
        }
    }
}

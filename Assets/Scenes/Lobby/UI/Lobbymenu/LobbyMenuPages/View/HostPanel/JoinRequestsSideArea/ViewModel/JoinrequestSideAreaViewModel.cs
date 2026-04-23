using Common.Services.Net.Modules;
using Common.systems.MainThread;
using Common.systems.ProfileSystem.Entities;
using Newtonsoft.Json;
using Scenes.Lobby;
using System;
using System.Threading.Tasks;
using UnityEditor.Search;
using UnityEngine;
using Zenject;


public class JoinRequestsSizeAreaViewModel
{
    private WebSocketModule _socket;
    [Inject] private MainThreadDispatcher _dispatcher;
    [Inject] private DiContainer _container;
    [Inject] private LobbyManager _lobbyManager;

    public event Action<Profile> onNewRequest;

    public JoinRequestsSizeAreaViewModel(WebSocketModule socket)
    {
        _socket = socket;
        _socket.On("requestToJoin", handleRequestToJoin);
    }

    public void cleanUp()
    {
        _socket.Off("requestToJoin", handleRequestToJoin);
    }

    private async Task handleRequestToJoin(string arg)
    {
        Debug.Log("ПРИШЕЛ ЗАПРОС)");
        Debug.Log(arg);
        Profile ProfileRequest = JsonConvert.DeserializeObject<Profile>(arg);
        _dispatcher.Run(() =>
        {
            onNewRequest?.Invoke(ProfileRequest);
        });
    }

    internal void ApplyRequest(Profile profile)
    {
        ApplyRequestDTO dto = new ApplyRequestDTO { LobbyId = _lobbyManager.Lobby.Id, UserId = profile.UserId };
        _ = _socket.Send("ApplyPlayerJoinRequest", dto);
    }
}

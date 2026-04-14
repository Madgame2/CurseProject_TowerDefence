using Common.Events;
using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.systems.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scenes.Lobby.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class JoinToLobbyViewModel
{
    private readonly UIManager _uiManager;
    private readonly WebSocketModule _socket;

    private Dictionary<string, Scenes.Lobby.Entities.Lobby> availableLobbies = new();

    public Action<Scenes.Lobby.Entities.Lobby[]> InitArray;
    public Action<string> LobbyRemover;
    public Action<Scenes.Lobby.Entities.Lobby> LobbyCreated;
    public Action<string, Scenes.Lobby.Entities.Lobby> LobbyUpdated;
    public Action timeOut;

    public JoinToLobbyViewModel(UIManager uiManager, NetService netService)
    {
        _uiManager = uiManager;
        _socket = netService._webSocketModule;
        Init();
    }

    internal void onClose()
    {
        _uiManager.Close("JoinToLobby");
    }

    private async void Init()
    {
        availableLobbies.Clear();
        try
        {
            await LoadAvailableLobbies();
            await SubscribeToLobbyEvents();
        }
        catch (OperationCanceledException)
        {
            Debug.LogError("Timeout: не удалось получить лобби или подписаться на события");
            timeOut?.Invoke();
        }
    }


    private async Task LoadAvailableLobbies()
    {
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(25)))
        {
            var result = await _socket.SendRequest(
                "GetAvailableLobbies",
                new { },
                cts.Token
            );

            if (result.Code != 200)
            {
                throw new Exception("Failed to load lobbies");
            }

            var json = result.Data.ToString();
            var lobbies = JsonConvert.DeserializeObject<Scenes.Lobby.Entities.Lobby[]>(json);

            foreach(var lobby in lobbies)
            {
                availableLobbies.Add(lobby.Id, lobby);
            }
            InitArray?.Invoke(availableLobbies.Values.ToArray());
        }
    }

    private async Task SubscribeToLobbyEvents()
    {
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
        {
            await _socket.Send("Events/SubScribeLobiesEvents", new { }, cts.Token);

            _socket.On("LobbysUpdated", lobbyUpdateHandler);

            Debug.Log("Subscribed to lobby events");
        }
    }

    public async Task ClearAll()
    {
        try
        {
            _socket.Off("LobbysUpdated", lobbyUpdateHandler);

            await _socket.Send("Events/UnubScribeLobiesEvents", new { });
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"ClearAll failed: {ex.Message}");
        }
    }

    private void lobbyUpdateHandler(string obj)
    {
        Debug.Log(obj);

        var baseEvent = JsonConvert.DeserializeObject<LobbyEvent>(obj);

        switch (baseEvent.type)
        {
            case "LOBBY_CREATED":
                HandleCreated(baseEvent);
                break;

            case "LOBBY_UPDATED":
                HandleUpdated(baseEvent);
                break;

            case "LOBBY_DELETED":
                HandleDeleted(baseEvent);
                break;
        }
    }

    private void HandleDeleted(LobbyEvent baseEvent)
    {
        string deletedLobbyId = baseEvent.lobbyId;
        availableLobbies.Remove(deletedLobbyId);

        LobbyRemover?.Invoke(deletedLobbyId);
    }

    private void HandleUpdated(LobbyEvent baseEvent)
    {
        string UpdatedLobbyId = baseEvent.lobbyId;
        if (!availableLobbies.ContainsKey(UpdatedLobbyId)) return;

        LobbyUpdated?.Invoke(baseEvent.lobbyId, baseEvent.lobby);
    }

    private void HandleCreated(LobbyEvent baseEvent)
    {
        string createdLobbyId = baseEvent.lobbyId;
        if (availableLobbies.ContainsKey(createdLobbyId)) return;

        LobbyCreated?.Invoke(baseEvent.lobby);
            
    }
}

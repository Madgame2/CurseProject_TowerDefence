using Common.Services.Net.Modules;
using Scenes.Lobby.Entities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbyShowElem : MonoBehaviour
{
    [SerializeField] private Image _hostImage;
    [SerializeField] private TMP_Text _hotName;
    [SerializeField] private TMP_Text _playersCount;
    [SerializeField] private Button _joinButton;

    private Scenes.Lobby.Entities.Lobby _lobby;

    [Inject] private WebSocketModule _socket;

    public void Init(Scenes.Lobby.Entities.Lobby lobby)
    {
        _lobby = lobby;
        _hotName.text = lobby.hostName;
        _playersCount.text = $"{lobby.Users.Count}/{lobby.MaxSize}";

        _joinButton.onClick.AddListener(onPuttonClickhandler);
    }

    internal void clearUp()
    {
        _joinButton.onClick.RemoveAllListeners();
    }

    void onPuttonClickhandler()
    {
        _= _socket.SendRequest("LobbyrequestToJoin", new RequestJoinToLobbyDTO { LobbyID = _lobby.Id });
    }
}

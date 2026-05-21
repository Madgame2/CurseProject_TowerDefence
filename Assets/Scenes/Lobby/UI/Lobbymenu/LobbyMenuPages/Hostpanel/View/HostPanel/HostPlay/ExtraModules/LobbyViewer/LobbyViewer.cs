using Common.Services.Net.Modules;
using NUnit.Framework;
using Scenes.Lobby;
using Scenes.Lobby.Entities;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Zenject;

public class LobbyViewer : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private List<LobbyViewerElem> elements;
    [SerializeField] private GameObject _template;

    [Inject] private LobbyManager _lobbyManager;
    [Inject] private WebSocketModule _socket;

    private void Start()
    {
        Render();

        _lobbyManager.onLobbyUpdated += HandleLobbyUpdated;
    }
    private void OnDisable()
    {
        _lobbyManager.onLobbyUpdated -= HandleLobbyUpdated;

    }

    private void HandleLobbyUpdated(Scenes.Lobby.Entities.Lobby lobby)
    {
        Render();
    }

    private void Render()
    {
        foreach (Transform child in root)
        {
            GameObject.Destroy(child.gameObject);
        }

        var lobby = _lobbyManager.Lobby;
        foreach (var item in lobby.LobbyUsers)
        {
            GameObject newObject = Instantiate(_template, root);
            if (newObject.TryGetComponent<LobbyViewerElem>(out LobbyViewerElem elemView))
            {
                elemView.Init(item.PlayerId,item.Name,item.avatarSource,item.PlayerId == lobby.Host,
                        async (userId) =>
                        {
                            _ = _socket.Send(
                                "RemoveUserFromLobby",
                                new RemoveUserDTO(userId)
                            );
                        });
            }
        }
    }
}

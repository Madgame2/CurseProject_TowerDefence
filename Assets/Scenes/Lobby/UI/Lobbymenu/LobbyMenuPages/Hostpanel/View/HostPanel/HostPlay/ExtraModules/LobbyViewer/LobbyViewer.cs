using NUnit.Framework;
using Scenes.Lobby;
using Scenes.Lobby.Entities;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LobbyViewer : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private List<LobbyViewerElem> elements;
    [SerializeField] private GameObject _template;

    [Inject] private LobbyManager _lobbyManager;

    private void Start()
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
                elemView.Init(item.Name,item.PlayerId == lobby.Host);
            }
        }
    }
}

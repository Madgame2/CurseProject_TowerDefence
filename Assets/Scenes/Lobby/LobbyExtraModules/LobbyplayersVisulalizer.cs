using Scenes.Lobby;
using UnityEngine;
using Zenject;

public class LobbyplayersVisulalizer : MonoBehaviour
{
    [SerializeField] private GameObject _hostAvata;
    [SerializeField] private GameObject[] _memberAvatar;

    [Inject] private LobbyManager _lobbyManager;

    public void Start()
    {
        _hostAvata.SetActive(false);

        foreach (var member in _memberAvatar)
        {
            member.SetActive(false);
        }

        _lobbyManager.onLobbyUpdated += onLobbyUpdateHandler;
    }
    public void OnDisable()
    {
        _lobbyManager.onLobbyUpdated -= onLobbyUpdateHandler;
    }

    public void OnDestroy()
    {
        _lobbyManager.onLobbyUpdated -= onLobbyUpdateHandler;
    }

    private void onLobbyUpdateHandler(Scenes.Lobby.Entities.Lobby lobby)
    {
        int newPlayerCount = lobby.Users.Count;
        _hostAvata.SetActive(newPlayerCount >= 1);

        Debug.Log(newPlayerCount);
        for (int i = 0; i < _memberAvatar.Length; i++)
        {
            bool shouldBeActive = (i + 2) <= newPlayerCount;
            _memberAvatar[i].SetActive(shouldBeActive);
        }
    }
}

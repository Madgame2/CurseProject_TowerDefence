using Common.systems.ProfileSystem;
using Common.systems.ProfileSystem.Entities;
using Common.systems.UI.PagesSystem;
using Scenes.Lobby;

using UnityEngine;

public class LobbyViewModel
{
    private PagesContainer _pagesContainer;
    private readonly LobbyManager _lobbyManager;
    private readonly ProfileManager _profileManager;

    public LobbyViewModel(LobbyManager lobbyManager, ProfileManager profileManager)
    {

        _lobbyManager = lobbyManager;
        _profileManager = profileManager;

        _lobbyManager.onLobbyUpdated += HandleChanges;
        _profileManager.onProfileUpdated += HandleChanges;
    }

    public void cleanUp() {
        _lobbyManager.onLobbyUpdated -= HandleChanges;
        _profileManager.onProfileUpdated -= HandleChanges;
    }

    public void InitViewModel(PagesContainer pagesContainer)
    {
        _pagesContainer = pagesContainer;
    }

    private void HandleChanges()
    {
        if (_lobbyManager.Lobby == null)
        {
            Debug.LogError("Не реализовано когда нет лобби");
            return;
        }

        if (_profileManager.Profile == null)
        {
            Debug.LogError("Не реализовано когда нет профиля");
            return;
        }

        Scenes.Lobby.Entities.Lobby lobby = _lobbyManager.Lobby;
        Profile profile = _profileManager.Profile;
        if (lobby.Host == profile.UserId)
        {
            _pagesContainer.OpenPageByName("HostPanel");
        }
        else
        {
            Debug.LogError("Не реализована панель когда не хост");
        }
    }
}

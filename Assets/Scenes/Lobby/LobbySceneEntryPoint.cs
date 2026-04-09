using Common.systems.ProfileSystem;
using Scenes.Lobby;
using UnityEngine;
using Zenject;

public class LobbySceneEntryPoint : IInitializable
{
    private readonly ProfileManager _profileManager;
    private readonly LobbyManager _lobbyManager;

    public LobbySceneEntryPoint(ProfileManager profileManager, LobbyManager lobbyManager)
    {
        _profileManager = profileManager;
        _lobbyManager = lobbyManager;
    }

    public async void Initialize()
    {
        await _profileManager.Init();
        await _lobbyManager.Init();
    }
}

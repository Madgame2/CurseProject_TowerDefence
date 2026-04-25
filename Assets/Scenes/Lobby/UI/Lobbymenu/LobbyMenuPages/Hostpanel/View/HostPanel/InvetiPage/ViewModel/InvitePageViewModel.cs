using Common.systems.UI;
using Scenes.Lobby;
using System;
using UnityEngine;

public class InvitePageViewModel
{
    private readonly LobbyManager _lobbyManager;
    private readonly UIManager _uiMannager;
    private string _inviteCode;


    public InvitePageViewModel(LobbyManager lobbyManager, UIManager uIManager)
    {
        _lobbyManager = lobbyManager;
        _uiMannager = uIManager;
    }

    internal string InitCode()
    {
        _inviteCode = _lobbyManager.Lobby.inviteCode;
        return _inviteCode;
    }

    internal void onClose()
    {
        _uiMannager.Close("InvitePage");
    }

    internal void onCopy()
    {
        GUIUtility.systemCopyBuffer = _inviteCode;
    }
}

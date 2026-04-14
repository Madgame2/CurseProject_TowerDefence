using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyShowElem : MonoBehaviour
{
    [SerializeField] private Image _hostImage;
    [SerializeField] private TMP_Text _hotName;
    [SerializeField] private TMP_Text _playersCount;
    [SerializeField] private Button _joinButton;


    public void Init(Scenes.Lobby.Entities.Lobby lobby)
    {
        _hotName.text = lobby.hostName;
        _playersCount.text = $"{lobby.Users.Count}/{lobby.MaxSize}";
    }

    internal void clearUp()
    {
        
    }
}

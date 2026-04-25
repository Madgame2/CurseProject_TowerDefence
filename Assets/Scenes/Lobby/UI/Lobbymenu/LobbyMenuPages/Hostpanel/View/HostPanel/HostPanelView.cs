using Common.systems.MainThread;
using Common.systems.UI.View;
using Scenes.Lobby;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HostPanelView : ViewBase<HostPanelViewModel>
{
    [SerializeField] private Button _play;
    [SerializeField] private Button _joinToLobby;
    [SerializeField] private Button _invite;
    [SerializeField] private Button onBack;
    [Inject] private readonly MainThreadDispatcher _dispatcher;

    [Inject] private LobbyManager _lobbyManager;

    private bool _isSearching = false;          // флаг состояния поиска
    private bool _lastButtonsAvailable = false; // последнее значение из события ChangeButtonsAvailable
    protected override void OnViewModelAssigned()
    {
        onBack.onClick.AddListener(ViewModel.onBack);
        _play.onClick.AddListener(ViewModel.onPlayButton);
        _invite.onClick.AddListener(ViewModel.onInvite);
        _joinToLobby.onClick.AddListener(ViewModel.onJoinToLobby);

        ViewModel.ChangeButtonsAvailable += setButtonsActive;

        _lobbyManager.gameSearchStateChanges += SearchGameHandler;
    }

    public override void Cleanup()
    {
        onBack.onClick.RemoveAllListeners();
        _play.onClick.RemoveAllListeners();
        _invite.onClick.RemoveAllListeners();
        _joinToLobby.onClick.RemoveAllListeners();

        ViewModel.ChangeButtonsAvailable -= setButtonsActive;
        _lobbyManager.gameSearchStateChanges -= SearchGameHandler;
    }

    private void SearchGameHandler(bool searchState)
    {
        // Если состояние не изменилось, ничего не делаем
        if (_isSearching == searchState) return;

        if (searchState)
        {
            _isSearching = true;
            _dispatcher.Run(() =>
            {
                var playText = _play.GetComponentInChildren<TMP_Text>();
                if (playText != null) playText.text = "Cancel";

                _play.onClick.RemoveListener(ViewModel.onPlayButton);
                _play.onClick.AddListener(ViewModel.OnCancelSearchClicked);

                _invite.interactable = false;
                _joinToLobby.interactable = false;
            });
            setButtonsActive(_lastButtonsAvailable);
        }
        else
        {
            _isSearching = false;
            _dispatcher.Run(() =>
            {
                var playText = _play.GetComponentInChildren<TMP_Text>();
                if (playText != null) playText.text = "Play";

                _play.onClick.RemoveListener(ViewModel.OnCancelSearchClicked);
                _play.onClick.AddListener(ViewModel.onPlayButton);

                _invite.interactable = _lastButtonsAvailable;
                _joinToLobby.interactable = _lastButtonsAvailable;
            });
            setButtonsActive(_lastButtonsAvailable);
        }
    }


    private void setButtonsActive(bool active)
    {
        _lastButtonsAvailable = active;
        onBack.interactable = active;
        _play.interactable = active;

        if (!_isSearching)
        {
            _joinToLobby.interactable = active;
            _invite.interactable = active;
        }
        else
        {
            // Во время поиска эти кнопки всегда неактивны
            _joinToLobby.interactable = false;
            _invite.interactable = false;
        }
    }
}

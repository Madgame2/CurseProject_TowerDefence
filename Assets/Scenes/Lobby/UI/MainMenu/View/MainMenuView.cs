using Common.systems.UI;
using Common.systems.UI.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuView: ViewBase<MainMenuViewModel>
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;



    protected override void OnViewModelAssigned()
    {
        _playButton.onClick.AddListener(ViewModel.GotoLobbyPage);
        _settingsButton.onClick.AddListener(ViewModel.GotoSettings);
        _exitButton.onClick.AddListener(ViewModel.Exit);

        ViewModel.ChangeButtonsAvailable += setButtonsActive;
    }

    public override void Cleanup()
    {
        _playButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();

        ViewModel.ChangeButtonsAvailable -= setButtonsActive;
    }

    private void setButtonsActive(bool active)
    {
        _playButton.interactable = active;
        _settingsButton.interactable = active;
        _exitButton.interactable = active;
    }

    public void OnButtonClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

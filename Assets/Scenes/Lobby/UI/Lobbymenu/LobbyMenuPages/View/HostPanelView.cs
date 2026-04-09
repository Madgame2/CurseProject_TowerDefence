using Common.systems.UI.View;
using UnityEngine;
using UnityEngine.UI;

public class HostPanelView : ViewBase<HostPanelViewModel>
{
    [SerializeField] private Button _play;
    [SerializeField] private Button _joinToLobby;
    [SerializeField] private Button _invite;
    [SerializeField] private Button onBack;
    protected override void OnViewModelAssigned()
    {
        onBack.onClick.AddListener(ViewModel.onBack);

        ViewModel.ChangeButtonsAvailable += setButtonsActive;
    }

    public override void Cleanup()
    {
        onBack.onClick.RemoveAllListeners();

        ViewModel.ChangeButtonsAvailable -= setButtonsActive;
    }

    private void setButtonsActive(bool active)
    {
        onBack.interactable = active;
        _play.interactable = active;
        _joinToLobby.interactable = active;
        _invite.interactable = active;
    }
}

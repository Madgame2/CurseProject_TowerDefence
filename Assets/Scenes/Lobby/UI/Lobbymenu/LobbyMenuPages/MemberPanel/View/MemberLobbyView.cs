using Common.systems.UI.View;
using UnityEngine;
using UnityEngine.UI;

public class MemberLobbyView : ViewBase<MemberLobbyVewModel>
{
    [SerializeField] private Button _inviteButton;
    [SerializeField] private Button _leaveTheLobbyButton;
    [SerializeField] private Button _backButton;

    protected override void OnViewModelAssigned()
    {
        _inviteButton.onClick.AddListener(ViewModel.InviteButtonHandler);
        _leaveTheLobbyButton.onClick.AddListener(ViewModel.LeavTheLobbyHandler);
        _backButton.onClick.AddListener(ViewModel.onBackHandler);

        ViewModel.ChangeButtonsAvailable += setButtonsActive;
    }

    public override void Cleanup()
    {
        _inviteButton.onClick.RemoveAllListeners();
        _leaveTheLobbyButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();

        ViewModel.ChangeButtonsAvailable -= setButtonsActive;
    }

    private void setButtonsActive(bool active)
    {
        _inviteButton.interactable = active;
        _leaveTheLobbyButton.interactable = active;
        _backButton.interactable = active;
    }
}

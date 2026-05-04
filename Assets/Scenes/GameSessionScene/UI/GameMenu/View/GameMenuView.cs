using Common.systems.UI.View;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuView : ViewBase<gameMenuViewModel>
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _leaveFromSession;
    [SerializeField] private Button _exitToTheDesktop;

    protected override void OnViewModelAssigned()
    {
        _resumeButton.onClick.AddListener(ViewModel.OnResumeButtonHandler);
        _leaveFromSession.onClick.AddListener(ViewModel.OnLeaveFromSessionHandler);
        _exitToTheDesktop.onClick.AddListener(ViewModel.OnExitToDescktopHandler);
    }


    public override void Cleanup()
    {
        _resumeButton.onClick.RemoveAllListeners();
        _leaveFromSession.onClick.RemoveAllListeners();
        _exitToTheDesktop.onClick.RemoveAllListeners();
    }
}

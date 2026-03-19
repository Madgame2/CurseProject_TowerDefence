using Common.systems.UI.View;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccountView : ViewBase<CreateAccountViewModel>
{
    [SerializeField] private Button LogInButton;
    [SerializeField] private Button ExitButton;

    protected override void OnViewModelAssigned()
    {
        LogInButton.onClick.AddListener(LogInButtonHandler);
        ExitButton.onClick.AddListener(ExitButtonHandler);
    }

    public override void Cleanup()
    {
        LogInButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();
    }

    private void LogInButtonHandler()
    {
        ViewModel.swapToLogIN();
    }
    private void ExitButtonHandler()
    {
        ViewModel.CloseGame();
    }

}

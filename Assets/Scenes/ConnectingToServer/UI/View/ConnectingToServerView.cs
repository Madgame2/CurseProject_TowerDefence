using Common.systems.UI;
using Common.systems.UI.View;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingToServerView :ViewBase<ConnectingToServerViewModel>
{
    [SerializeField] private Button SubmitButton;
    [SerializeField] private Button RegistryButton;
    [SerializeField] private Button ExitButton;


    protected override void OnViewModelAssigned()
    {
        SubmitButton.onClick.AddListener(Submit_onPress);
        RegistryButton.onClick.AddListener(Registry_onPress);
        ExitButton.onClick.AddListener( Exit_onPress);
    }

    public override void Cleanup()
    {
        SubmitButton.onClick.RemoveAllListeners();
        RegistryButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();
    }

    private void Submit_onPress()
    {

    }

    private void Registry_onPress()
    {
        ViewModel.swapToCreateAccountView();
    }

    private void Exit_onPress()
    {
        ViewModel.exitFormGame();
    }
}

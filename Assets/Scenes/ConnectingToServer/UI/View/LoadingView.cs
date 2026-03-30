using UnityEngine;
using Common.systems.UI.View;
using TMPro;

public class LoadingView : ViewBase<LoadingViewModel>
{
    [SerializeField] private TMP_Text message;

    protected override void OnViewModelAssigned()
    {
        ViewModel.onMessageUpdate += OnMessageUpdate;
    }

    public override void Cleanup()
    {
        ViewModel.onMessageUpdate += OnMessageUpdate;
    }

    private void OnMessageUpdate(string newMessage) {

        message.text = newMessage;
    }    
}

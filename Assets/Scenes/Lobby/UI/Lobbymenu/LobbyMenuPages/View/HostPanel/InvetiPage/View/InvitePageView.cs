using Common.systems.UI.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvitePageView : ViewBase<InvitePageViewModel>
{
    [SerializeField] private TMP_InputField _codeArea;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _copyButton;

    protected override void OnViewModelAssigned()
    {
        _closeButton.onClick.AddListener(ViewModel.onClose);
        _copyButton.onClick.AddListener(ViewModel.onCopy);

        _codeArea.text = ViewModel.InitCode();
    }

    public override void Cleanup()
    {
        _closeButton.onClick.RemoveAllListeners();
        _copyButton.onClick.RemoveAllListeners();
    }
}

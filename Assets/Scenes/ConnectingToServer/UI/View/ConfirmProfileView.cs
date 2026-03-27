using Common.systems.UI.View;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmProfileView : ViewBase<ConfirmProfileViewModel>
{
    [SerializeField] private GameObject _rootPanel;
    [SerializeField] private Button _cancelButton;

    [SerializeField] private TMP_Text _descriptionFeald;

    [SerializeField] private TMP_InputField _codeInput;
    [SerializeField] private Button _submitButton;


    protected override void OnViewModelAssigned()
    {
        _descriptionFeald.text = $"We’ve sent a verification code to your email address: {ViewModel.Email}.\r\n\r\nTo complete the verification process and secure your account, please open your email inbox, locate the message with the code, and carefully enter it in the field below.\r\n\r\nMake sure to check your spam or junk folder if you don’t see the email in your main inbox.\r\nOnce you enter the correct code, your account will be successfully verified and you’ll be able to continue using all features.";

        _cancelButton.onClick.AddListener(processClose);
        _submitButton.onClick.AddListener(processSubmit);
        ViewModel.SetVisible += processVisible;
        
    }
    public override void Cleanup()
    {
        ViewModel.SetVisible -= processVisible;
        _cancelButton.onClick.RemoveAllListeners();
        _submitButton.onClick.RemoveAllListeners();
    }

    private async void processSubmit()
    {
        if (string.IsNullOrEmpty(_codeInput.text))
        {

        }

        bool isCorrect = await ViewModel.tryVerifyUser(_codeInput.text);

        if (!isCorrect)
        {

        }
    }
    private void processVisible(bool visible)
    {
        if (visible) show();
        else hide();
    }

    private void hide()
    {
        _rootPanel.SetActive(false);
    }

    private void show()
    {
        _rootPanel.SetActive(true);
    }

    private void processClose()
    {
        ViewModel.closeWindow();
    }
}

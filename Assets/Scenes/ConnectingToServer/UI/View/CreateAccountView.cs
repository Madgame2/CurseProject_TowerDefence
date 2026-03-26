using Common.Exceptions.DTO;
using Common.Exceptions.enums;
using Common.systems.UI.View;
using ModestTree;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccountView : ViewBase<CreateAccountViewModel>
{
    [SerializeField] private Button LogInButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button SubmitButton;

    [SerializeField] private TMP_InputField _nickname;
    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;

    [SerializeField] private TMP_Text _nicknameError;
    [SerializeField] private TMP_Text _emailError;
    [SerializeField] private TMP_Text _passwordError;


    protected override void OnViewModelAssigned()
    {
        LogInButton.onClick.AddListener(LogInButtonHandler);
        ExitButton.onClick.AddListener(ExitButtonHandler);
        SubmitButton.onClick.AddListener(submitButton);

        ViewModel.onError += processErrors;
    }

    public override void Cleanup()
    {
        LogInButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();
        SubmitButton.onClick.RemoveAllListeners();

        ViewModel.onError -= processErrors;
    }

    private void processErrors(RegisterError err)
    {
        switch (err.errorType)
        {
            case RegisterErrorsEnums.nicknameError:
                _nicknameError.text = err.message;
                break;
            case RegisterErrorsEnums.emailError:
                _emailError.text = err.message;
                break;
            case RegisterErrorsEnums.passwordError:
                _passwordError.text = err.message;
                break;
        }
    }


    private async void submitButton()
    {
        _passwordError.text = "";
        _emailError.text = "";
        _nicknameError.text = "";

        await ViewModel.Submit(_nickname.text, _email.text, _password.text);
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

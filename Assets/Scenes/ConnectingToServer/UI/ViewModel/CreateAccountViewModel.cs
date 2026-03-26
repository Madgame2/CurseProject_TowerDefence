using Common.Exceptions.DTO;
using Common.Exceptions.enums;
using Common.Services.Net;
using Common.Services.Net.Services;
using Common.systems.GameStates;
using Common.systems.UI;
using ModestTree;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using UnityEditor.Search;
using UnityEngine;

public class CreateAccountViewModel
{
    private readonly GameStateMachine gameStateMachine;
    private readonly UIManager uIManager;
    private readonly NetService netService;

    public event Action<RegisterError> onError;

    public CreateAccountViewModel(GameStateMachine gameStateMachine, UIManager uIManager, NetService netService, UIManager ui)
    {
        this.gameStateMachine = gameStateMachine;
        this.uIManager = uIManager;
        this.netService = netService;
    }

    public async Task Submit(string nickname, string email, string password)
    {
        nickname = nickname.Trim();
        if (nickname.IsEmpty())
        {
            onError?.Invoke(new RegisterError(RegisterErrorsEnums.nicknameError, "nickname can not be empty"));
            return;
        }

        email = email.Trim();
        if (email.IsEmpty())
        {
            onError?.Invoke(new RegisterError(RegisterErrorsEnums.emailError, "email can not be empty"));
            return;
        }
        else if (!IsValidEmail(email))
        {
            onError?.Invoke(new RegisterError(RegisterErrorsEnums.emailError, "email is not valid"));
            return;
        }

        password = password.Trim();
        if (password.IsEmpty())
        {
            onError?.Invoke(new RegisterError(RegisterErrorsEnums.passwordError, "password can not be empty"));
            return;
        }



        var service = netService.CreateAuthService();

        var responce = await service.RegisterPlayer(nickname, email, password);

        Debug.Log(responce.StatusCode);

        switch (responce.StatusCode)
        {
            case 200:
                ShowConfirmProfileWindow(email);
                break;
            case 400:

                break;
            case 409:

                break;
        }
    }

    public void ShowConfirmProfileWindow(string email)
    {
        ConfirmProfileViewModel vm = (ConfirmProfileViewModel)uIManager.TryOpen("ConfirmProfile");
        vm.init(email);
    }


    public void swapToLogIN()
    {
        uIManager.Close("CreateAccount");
        uIManager.TryOpen("LogIn");
    }

    public void CloseGame()
    {
        gameStateMachine.tryMoveToState(typeof(ExitState));
    }



    public bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

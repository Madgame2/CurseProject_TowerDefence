using Common.Exceptions.DTO;
using Common.Exceptions.enums;
using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.Services.Net.Services;
using Common.systems.GameStates;
using Common.systems.UI;
using Common.Validation;
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
        email = email.Trim();
        password = password.Trim();

        if (!Validator.ValidateNickname(nickname, onError)) return;
        if (!Validator.ValidateEmail(email, onError)) return;
        if (!Validator.ValidatePassword(password, onError)) return;

        ShowLoading();

        try
        {
            var response = await Register(nickname, email, password);
            HandleResponse(response, email);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
        finally
        {
            HideLoading();
        }
    }


    private void HandleResponse(HttpResponse response, string email)
    {
        switch (response.StatusCode)
        {
            case 200:
                HandleSuccess(email);
                break;

            case 400:
                HandleBadRequest(response);
                break;

            case 409:
                HandleConflict(response);
                break;

            default:
                HandleUnknownError(response);
                break;
        }
    }

    private void HandleException(Exception ex)
    {
        Debug.LogError("Надо обработать потерю соединения");
        Debug.LogException(ex);
    }

    private void HandleSuccess(string email)
    {
        ShowConfirmProfileWindow(email);
    }

    private void HandleBadRequest(HttpResponse response)
    {
        Debug.LogError("400 Bad Request - not implemented");
        throw new NotImplementedException();
    }

    private void HandleConflict(HttpResponse response)
    {
        Debug.LogError("409 Conflict - not implemented");
        throw new NotImplementedException();
    }

    private void HandleUnknownError(HttpResponse response)
    {
        Debug.LogError($"Unknown status code: {response.StatusCode}");
        throw new NotImplementedException();
    }


    private async Task<HttpResponse> Register(string nickname, string email, string password)
    {
        var service = netService.CreateAuthService();
        return await service.RegisterPlayer(nickname, email, password);
    }


    private void ShowLoading()
    {
        uIManager.TryOpen("Loading").Hide("CreateAccount");
    }
    private void HideLoading()
    {
        uIManager.Close("Loading");
        uIManager.Show("CreateAccount");
    }


    //public async Task Submit(string nickname, string email, string password)
    //{
    //    nickname = nickname.Trim();
    //    if (nickname.IsEmpty())
    //    {
    //        onError?.Invoke(new RegisterError(RegisterErrorsEnums.nicknameError, "nickname can not be empty"));
    //        return;
    //    }

    //    email = email.Trim();
    //    if (email.IsEmpty())
    //    {
    //        onError?.Invoke(new RegisterError(RegisterErrorsEnums.emailError, "email can not be empty"));
    //        return;
    //    }
    //    else if (!IsValidEmail(email))
    //    {
    //        onError?.Invoke(new RegisterError(RegisterErrorsEnums.emailError, "email is not valid"));
    //        return;
    //    }

    //    password = password.Trim();
    //    if (password.IsEmpty())
    //    {
    //        onError?.Invoke(new RegisterError(RegisterErrorsEnums.passwordError, "password can not be empty"));
    //        return;
    //    }



    //    var service = netService.CreateAuthService();
    //    uIManager.TryOpen("Loading").Hide("CreateAccount");
    //    try
    //    {
    //        var responce = await service.RegisterPlayer(nickname, email, password);


    //    switch (responce.StatusCode)
    //    {
    //        case 200:
    //            ShowConfirmProfileWindow(email);
    //            break;
    //        case 400:

    //            break;
    //        case 409:

    //            break;
    //    }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("Надо обработать потерю соединения");
    //    }
    //    uIManager.Close("Loading");
    //    uIManager.Show("CreateAccount");
    //}

    public void ShowConfirmProfileWindow(string email)
    {
        
        uIManager.TryOpen("ConfirmProfile" ,out object vm);
        ConfirmProfileViewModel viewModel = (ConfirmProfileViewModel)vm;

        viewModel.init(email);
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

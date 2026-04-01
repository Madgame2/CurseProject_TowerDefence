using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.Services.Net.Services;
using Common.systems.GameStates;
using Common.systems.UI;
using Common.Validation;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;
using Common.Services.Tokens;

public class ConnectingToServerViewModel
{
    private readonly NetService netService;
    private readonly GameStateMachine gameStates;
    private readonly UIManager uIManager;
    public ConnectingToServerViewModel(GameStateMachine statesmachine, UIManager ui, NetService netService)
    {
        this.gameStates = statesmachine;
        this.uIManager = ui;
        this.netService = netService;
    }

    public void exitFormGame()
    {
        gameStates.tryMoveToState(typeof(ExitState));
    }

    public void swapToCreateAccountView()
    {
        uIManager.Close("LogIn");
        uIManager.TryOpen("CreateAccount");
    }

    public async Task Submit(string email, string password)
    {
        if (!Validator.ValidateEmail(email, null)) return;
        if(!Validator.ValidatePassword(password, null)) return;


        ShowLoading();

        try
        {
            var response = await Authorization(email, password);
            await HandleResponse(response);

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

    private async Task HandleResponse(HttpResponse response)
    {
        switch (response.StatusCode)
        {
            case 200:
                await Handle200(response);
                break;

            case 400:
                Handle400(response);
                break;

            case 403:
                Handle403(response);
                break;

            case 404:
                Handle404(response);
                break;

            case 500:
                Handle500(response);
                break;
        }
    }

    private void HandleException(Exception ex)
    {
        Debug.LogError("Надо обработать потерю соединения");
        Debug.LogException(ex);
    }

    private async Task Handle200(HttpResponse response)
    {
        AuthResponseDto dto = JsonConvert.DeserializeObject<AuthResponseDto>(response.Body);
        TokenManager.SetTokens(dto.accessToken, dto.refreshToken);

        var result = await netService._webSocketModule.tryConnect(dto.accessToken);

        if (result)
        {

            Debug.Log("connected");
            gameStates.tryMoveToState(typeof(LobbyState));
            return;
        }
        else
        {
            Debug.LogError("Не чет пошло не так");
        }

        await netService._webSocketModule.Disconnect();
    }

    private void Handle400(HttpResponse response)
    {
        Debug.LogError("400 Bad Request - handler not implemented");
        throw new NotImplementedException(nameof(Handle400));
    }

    private void Handle403(HttpResponse response)
    {
        Debug.LogError("403 Forbidden - handler not implemented");
        throw new NotImplementedException(nameof(Handle403));
    }

    private void Handle404(HttpResponse response)
    {
        Debug.LogError("404 Not Found - handler not implemented");
        throw new NotImplementedException(nameof(Handle404));
    }

    private void Handle500(HttpResponse response)
    {
        Debug.LogError("500 Internal Server Error - handler not implemented");
        throw new NotImplementedException(nameof(Handle500));
    }


    private async Task<HttpResponse> Authorization(string email, string password)
    {
        var service = netService.CreateAuthService();
        return await service.tryAuthorizatiUser(email, password);
    }

    private void ShowLoading()
    {
        uIManager.TryOpen("Loading").Hide("LogIn");
    }
    private void HideLoading()
    {
        uIManager.Close("Loading");
        uIManager.Show("LogIn");
    }
}

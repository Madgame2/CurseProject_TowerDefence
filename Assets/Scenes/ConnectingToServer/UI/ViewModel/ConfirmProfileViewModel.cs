using Common.Services.Net;
using Common.Services.Net.Services;
using Common.systems.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class ConfirmProfileViewModel
{
    private string profileEmail;

    private readonly UIManager _uiManager;
    private readonly NetService _netService;

    public event Action<bool> SetVisible;

    public string Email { get => profileEmail; }

    public ConfirmProfileViewModel(UIManager uiManager, NetService net)
    {
        _uiManager = uiManager;
        _netService = net;
    }

    internal void init(string email)
    {
        profileEmail =email;
    }


    public async Task closeWindow()
    {
        SetVisible?.Invoke(false);

        bool shouldClose = await _uiManager.QuestionWindow("Cancel confirmation?",
            "This action will cause the code sent to your account to no longer be valid. " +
            "You will be able to re-enter your details and receive a new code. Continue?",
            DialogType.Danger);


        if (shouldClose)
        {
            var authService =  _netService.CreateAuthService();
            var result = await authService.TryDeleteUnconfUser(profileEmail);

            switch (result.StatusCode)
            {
                case 400:
                    {
                        Debug.LogError("Not implemet");
                        break;
                    }
                case 500:
                    {
                        Debug.LogError("Not implemet");
                        break;
                    }
                case 404:
                    {
                        Debug.LogError("Not implemet");
                        break;
                    }
            }

            _uiManager.Close("ConfirmProfile");
            return;
        }

        SetVisible?.Invoke(true);

    }
}

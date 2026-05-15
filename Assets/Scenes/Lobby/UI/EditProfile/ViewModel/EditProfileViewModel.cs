using Common.Services.Net.Modules;
using Common.systems.ProfileSystem;
using Common.systems.UI;
using SFB;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Zenject;

public class EditProfileViewModel
{
    [Inject] private UIManager _uiManager;
    [Inject] private ProfileManager _profileManager;
    [Inject] private WebSocketModule _socket;


    public Action<string> onEmailChanged;
    public Action<string> onNicknameChanged;
    public Action<byte[]> aloadednewAvatar;

    private byte[] newAvatar;

    internal void Close()
    {
        _uiManager.Close("EditProfilePanel");
    }


    public void Init()
    {
        _profileManager.onProfileUpdated += HandlProfileUpdate;

        onEmailChanged?.Invoke(_profileManager.Profile.Email);
        onNicknameChanged?.Invoke(_profileManager.Profile.ProfileName);
    }

    private void HandlProfileUpdate()
    {
        onEmailChanged?.Invoke(_profileManager.Profile.Email);
        onNicknameChanged?.Invoke(_profileManager.Profile.ProfileName);
    }

    public void CleanUp()
    {
        _profileManager.onProfileUpdated -= HandlProfileUpdate;
    }

    internal void UploadNewavatar()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel(
            "Выберите изображение",
            "",
            new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") },
            false
        );

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            newAvatar = System.IO.File.ReadAllBytes(paths[0]);

            aloadednewAvatar?.Invoke(newAvatar);
        }
    }

    internal async Task SendNewData(string nickName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var dto = new ProfileData
        {
            nickName = nickName,
            userId = _profileManager.Profile.UserId,
            newImage = (newAvatar != null && newAvatar.Length > 0)
                ? newAvatar
                : null
        };

        await _socket.SendRequest("UploadNewUserData", dto, cancellationToken);
        _uiManager.Close("EditProfilePanel");
    }
}

using Common.systems.MainThread;
using Common.systems.ProfileSystem;
using Common.systems.UI;
using System;
using UnityEngine;
using Zenject;

public class UserInfoPanelViewModel
{
    [Inject] private ProfileManager _profileManager;
    [Inject] private UIManager _uIManager;
    [Inject] private MainThreadDispatcher mainThread;


    public Action<string> onNickNameChanged;
    public Action<string> onAvatarChanged;


    private bool _disposed;
    public void Init()
    {
        _profileManager.onProfileUpdated += handlerUpdatedProfile;

        _profileManager.SubscripeToProfileUpdate();
    }
    public void CleanUp()
    {
        _disposed = true;

        _profileManager.onProfileUpdated -= handlerUpdatedProfile;
    }

    private void handlerUpdatedProfile()
    {
        if (_disposed) return;

        mainThread.Run(() =>
        {
            onNickNameChanged?.Invoke(_profileManager.Profile.ProfileName);
            onAvatarChanged?.Invoke(_profileManager.Profile.avatarSource);
        });
    }

    internal void onAvatarButtonClick()
    {
        if (!_uIManager.IsOpen("PopUpPanel"))
        {
            _uIManager.TryOpen("PopUpPanel");
        }
    }
}

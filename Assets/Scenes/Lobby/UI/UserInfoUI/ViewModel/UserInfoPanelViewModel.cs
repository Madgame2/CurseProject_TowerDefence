using Common.systems.ProfileSystem;
using Common.systems.UI;
using System;
using UnityEngine;
using Zenject;

public class UserInfoPanelViewModel
{
    [Inject] private ProfileManager _profileManager;
    [Inject] private UIManager _uIManager;

    public Action<string> onNickNameChanged;




    public void Init()
    {
        _profileManager.onProfileUpdated += handlerUpdatedProfile;

        _profileManager.SubscripeToProfileUpdate();
    }
    public void CleanUp()
    {
        _profileManager.onProfileUpdated -= handlerUpdatedProfile;
    }

    private void handlerUpdatedProfile()
    {
        onNickNameChanged?.Invoke(_profileManager.Profile.ProfileName);
    }

    internal void onAvatarButtonClick()
    {
        if (!_uIManager.IsOpen("PopUpPanel"))
        {
            _uIManager.TryOpen("PopUpPanel");
        }
    }
}

using Common.systems.ProfileSystem;
using Common.systems.UI.View;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UserInfoPanelView : ViewBase<UserInfoPanelViewModel>
{
    [SerializeField] private TMP_Text _userNickname;
    [SerializeField] private AvatarLoader _profileAvatar;
    [SerializeField] private Button _avatarButton;

    private bool _isDestroyed;
    protected override void OnViewModelAssigned()
    {
        ViewModel.onNickNameChanged += handleNickNameChanded;
        ViewModel.onAvatarChanged += hadndleavatarChanged;

        _avatarButton.onClick.AddListener(ViewModel.onAvatarButtonClick);


        ViewModel.Init();
    }

    private void handleNickNameChanded(string obj)
    {
        _userNickname.text = obj;        
    }

    private void hadndleavatarChanged(string avatarSource)
    {
        if (_isDestroyed || !this)
            return;

        _profileAvatar.LoadFromUrl(avatarSource);
    }

    public override void Cleanup()
    {
        _isDestroyed = true;

        ViewModel.onNickNameChanged -= handleNickNameChanded;
        ViewModel.onAvatarChanged -= hadndleavatarChanged;


        _avatarButton.onClick.RemoveAllListeners();

        ViewModel.CleanUp();
    }
}

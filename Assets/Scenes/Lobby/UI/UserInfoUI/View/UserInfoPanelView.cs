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
    [SerializeField] private Image _profileAvatar;
    [SerializeField] private Button _avatarButton;
    protected override void OnViewModelAssigned()
    {
        ViewModel.onNickNameChanged += handleNickNameChanded;

        _avatarButton.onClick.AddListener(ViewModel.onAvatarButtonClick);

        ViewModel.Init();
    }

    private void handleNickNameChanded(string obj)
    {
        _userNickname.text = obj;        
    }

    public override void Cleanup()
    {
        ViewModel.onNickNameChanged -= handleNickNameChanded;

        _avatarButton.onClick.RemoveAllListeners();

        ViewModel.CleanUp();
    }
}

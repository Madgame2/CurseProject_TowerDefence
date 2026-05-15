using Common.systems.UI.View;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditProfileView : ViewBase<EditProfileViewModel>
{
    [SerializeField] private AvatarLoader _avatarImage;
    [SerializeField] private Button apploadNewAvatarButton;

    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _nickName;

    [SerializeField] private Button onCancel;
    [SerializeField] private Button onApply;

    private CancellationTokenSource _sendCts;


    protected override void OnViewModelAssigned()
    {
        onCancel.onClick.AddListener(ViewModel.Close);
        apploadNewAvatarButton.onClick.AddListener(ViewModel.UploadNewavatar);
        onApply.onClick.AddListener(HadnleapploadNeData);


        ViewModel.onEmailChanged += handleChandegEmail;
        ViewModel.onNicknameChanged += handleChangedNickname;
        ViewModel.aloadednewAvatar += handleNewAvatar;

        ViewModel.Init();
    }

    private async void HadnleapploadNeData()
    {
        _sendCts?.Cancel();
        _sendCts?.Dispose();

        _sendCts = new CancellationTokenSource();

        var timeoutCts = new CancellationTokenSource();
        timeoutCts.CancelAfter(5000);

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            _sendCts.Token,
            timeoutCts.Token
        );

        try
        {
            await ViewModel.SendNewData(_nickName.text, linkedCts.Token);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("SendNewData cancelled");
        }
        finally
        {
            linkedCts.Dispose();
            timeoutCts.Dispose();
        }
    }

    private void handleNewAvatar(byte[] obj)
    {
        _avatarImage.SetFromBytes(obj);
    }

    private void handleChangedNickname(string obj)
    {
        _nickName.text = obj;
    }

    private void handleChandegEmail(string obj)
    {
        _email.text = obj;
    }

    public override void Cleanup()
    {
        onCancel.onClick.RemoveAllListeners();
        apploadNewAvatarButton.onClick.RemoveAllListeners();

        ViewModel.onEmailChanged -= handleChandegEmail;
        ViewModel.onNicknameChanged -= handleChangedNickname;
        ViewModel.aloadednewAvatar -= handleNewAvatar;

        ViewModel.CleanUp();
    }
}

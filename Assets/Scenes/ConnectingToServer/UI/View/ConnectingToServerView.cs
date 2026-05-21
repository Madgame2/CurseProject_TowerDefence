using Common.systems.UI;
using Common.systems.UI.View;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingToServerView :ViewBase<ConnectingToServerViewModel>
{
    [SerializeField] private Button SubmitButton;
    [SerializeField] private Button RegistryButton;
    [SerializeField] private Button ExitButton;

    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;


    [SerializeField] private TMP_Text _emailError;
    [SerializeField] private TMP_Text _passwordError;
    protected override void OnViewModelAssigned()
    {
        SubmitButton.onClick.AddListener(Submit_onPress);
        RegistryButton.onClick.AddListener(Registry_onPress);
        ExitButton.onClick.AddListener( Exit_onPress);


        _emailError.gameObject.SetActive(false);
        _passwordError.gameObject.SetActive(false);

        ViewModel.onWrongEmail += handleEmailError;
        ViewModel.onWrongPassword += handleWrongPassword;
    }

    private void handleWrongPassword()
    {

        _passwordError.gameObject.SetActive(true);
    }

    private void handleEmailError()
    {

        _emailError.gameObject.SetActive(true);
    }

    public override void Cleanup()
    {
        SubmitButton.onClick.RemoveAllListeners();
        RegistryButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();

        ViewModel.onWrongEmail -= handleEmailError;
        ViewModel.onWrongPassword -= handleWrongPassword;
    }

    private void Submit_onPress()
    {
        _emailError.gameObject.SetActive(false);
        _passwordError.gameObject.SetActive(false);

        _ = ViewModel.Submit(_email.text, _password.text);
    }

    private void Registry_onPress()
    {
        ViewModel.swapToCreateAccountView();
    }

    private void Exit_onPress()
    {
        ViewModel.exitFormGame();
    }
}

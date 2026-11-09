using Common.systems.UI.View;
using TMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectToServerView : ViewBase<ConnectToServerViewModel>
{
    [Header("Connection")]
    [SerializeField] private TMP_InputField _host;
    [SerializeField] private TMP_InputField _sessionId;

    [Header("Actions")]
    [SerializeField] private Button _connect;
    [SerializeField] private Button _factConnect;

    protected override void OnViewModelAssigned()
    {
        base.OnViewModelAssigned();
        
        _connect.onClick.RemoveListener(OnConnectClicked);
        _factConnect.onClick.RemoveListener(OnFastConnectClicked);

        _connect.onClick.AddListener(OnConnectClicked);
        _factConnect.onClick.AddListener(OnFastConnectClicked);
    }

    private void OnConnectClicked()
    {
        ViewModel.Connect(_host.text, _sessionId.text);
    }

    private void OnFastConnectClicked()
    {
        ViewModel.QuickConnect();
    }

    public override void Cleanup()
    {
        _connect.onClick.RemoveListener(OnConnectClicked);
        _factConnect.onClick.RemoveListener(OnFastConnectClicked);

        base.Cleanup();
    }
}

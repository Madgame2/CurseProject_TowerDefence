using Common.Services.Net.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

public class ConnectToServerViewModel
{
    [Inject] private readonly INetworkClient _networkClient;

    private readonly TaskCompletionSource<bool> _connectionTcs =
        new TaskCompletionSource<bool>(
            TaskCreationOptions.RunContinuationsAsynchronously
        );

    public Task ConnectionTask => _connectionTcs.Task;

    public async Task Connect(string hostText, string sessionIdText)
    {
        if (_connectionTcs.Task.IsCompleted)
            return;

        var headers = new Dictionary<string, string>
        {
            { "EnterMode", "Development" },
            {"SessionId", sessionIdText}
        };

        bool connected = await _networkClient.TryCreateConnectionTo(
            hostText,
            headers
        );

        if (connected)
            _connectionTcs.TrySetResult(true);
    }

    public async Task QuickConnect()
    {
        if (_connectionTcs.Task.IsCompleted)
            return;

        await TryConnectToServer();
    }

    private async Task<bool> TryConnectToServer()
    {
        var headers = new Dictionary<string, string>
        {
            { "EnterMode", "Development" },
            {"SessionId", "Development"}
        };

        bool connected = await _networkClient.TryCreateConnectionTo(
            "127.0.0.1",
            5041,
            headers
        );

        if (connected)
            _connectionTcs.TrySetResult(true);

        return connected;
    }
}
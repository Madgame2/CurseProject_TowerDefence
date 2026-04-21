using Common.Services.Net;
using Common.Services.Net.Modules;
using Common.systems.ProfileSystem;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SessionNetInstaller
{
    private WebSocketModule _socket;
    [Inject] private ProfileManager _profileManager;

    public SessionNetInstaller(NetService net)
    {
        _socket = net._webSocketModule;
    }

    public async Task Install(SessionServerInfo sessionInfo)
    {

        var headers = new Dictionary<string, string>
                    {
                        { "Authorization", $"{sessionInfo.passToken}" },
                        { "X-User-Id", _profileManager.Profile.UserId },
                        { "x-session-id", sessionInfo.sessionId }
                    };

        ClientWebSocket newSocket = await WebSocketModule.CreateConnectionTo(sessionInfo.host, sessionInfo.port, headers);
        await _socket.ReplaceSessionSocketAsync(newSocket);
    }


    public void ClearAll()
    {

    }
}

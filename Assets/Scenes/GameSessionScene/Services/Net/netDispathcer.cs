using Common.Services.Net.Modules;
using Common.systems.MainThread;
using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class NetDispatcher
{
    [Inject] private WebSocketModule _socket;
    [Inject] private PlayersController _playersController;
    [Inject]private MainThreadDispatcher _mainThread;

    public NetDispatcher()
    {

    }

    public void Init()
    {
        _socket.On("world_update", handleEvents);
    }

    private async Task handleEvents(string arg)
    {
        WorldUpdateData packet = JsonConvert.DeserializeObject<WorldUpdateData>(arg);

        _mainThread.Run(() =>
        {
            if (packet.Players != null)
            {
                _playersController.handlePlayerNewState(packet.Players[0]);
            }
        });
    }
    public void cleanUp()
    {
        _socket.Off("world_update", handleEvents);
    }
}

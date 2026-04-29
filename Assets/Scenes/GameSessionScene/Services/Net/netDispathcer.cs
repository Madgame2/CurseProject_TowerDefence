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
        Debug.Log(arg);
        WorldUpdateData packet = JsonConvert.DeserializeObject<WorldUpdateData>(arg);

        if (packet?.Players == null || packet.Players.Count == 0)
            return;

        _mainThread.Run(() =>
        {
            foreach (var player in packet.Players)
            {
                if (player == null)
                    continue;

                _playersController.handlePlayerNewState(player);
            }
        });
    }
    public void cleanUp()
    {
        _socket.Off("world_update", handleEvents);
    }
}

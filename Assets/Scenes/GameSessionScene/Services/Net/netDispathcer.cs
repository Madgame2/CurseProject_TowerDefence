using Common.Services.Net.Modules;
using Common.systems.MainThread;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class NetDispatcher
{
    [Inject] private WebSocketModule _socket;
    [Inject] private PlayersController _playersController;
    [Inject] private MainThreadDispatcher _mainThread;
    [Inject] private EntityManager _entityManager;
    [Inject] private ChankSystem _chankSystem;
    [Inject] private NpcManager _npcManager;
    [Inject] private DirectorManager _director;
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
                if (player != null)
                    _playersController.handlePlayerNewState(player);
            }
            foreach(var chank in packet.chanks)
            {
                if (chank != null)
                    _chankSystem.handleChankUpdate(chank);
            }
            foreach(var entity in packet.enities)
            {
                _entityManager.handleEnityEvnet(entity);
            }
            foreach(var npc in packet.npc)
            {
                _npcManager.HandleNpcUpdate(npc);
            }
            foreach(var director in packet.director)
            {
                _director.HandleUpdate(director);
            }
        });
    }
    public void cleanUp()
    {
        _socket.Off("world_update", handleEvents);
    }
}

using Common.Services.Global;
using Common.Services.Net.Modules;
using Common.systems.MainThread;
using Common.systems.SceneStates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class NetDispatcher : IInitializable, IDisposable
{
    [Inject] private WebSocketModule _socket;
    [Inject] private PlayersController _playersController;
    [Inject] private MainThreadDispatcher _mainThread;
    [Inject] private EntityManager _entityManager;
    [Inject] private ChankSystem _chankSystem;
    [Inject] private NpcManager _npcManager;
    [Inject] private DirectorManager _director;
    [Inject] private SceneStateMachine<GameSessionScene> _sceneStateMachine;
    [Inject] private GlobalStorage _globalStorage;

    private static int _instanceCounter = 0;
    private int _instanceId;


    public NetDispatcher()
    {
        _instanceId = ++_instanceCounter;
        Debug.Log($"[NetDispatcher] CREATED id={_instanceId}");
    }

    public void Init()
    {
        _socket.On("world_update", handleEvents);
        _socket.On("sessionEnded", handleSessionEnd);

        Debug.Log($"[NetDispatcher] INIT id={_instanceId}");
    }

    private async Task handleEvents(string arg)
    {
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

    private async Task handleSessionEnd(string arg)
    {
        var dto = JsonConvert.DeserializeObject<SessionEndDTO>(arg);
        _globalStorage.Set<int>("WaveNum", dto.waveNum);
        _sceneStateMachine.tryMoveToState(typeof(EndSessionState));
    }

    public void cleanUp()
    {
        _socket.Off("world_update", handleEvents);
        _socket.Off("sessionEnded", handleSessionEnd);

    }

    public void Initialize()
    {
        _socket.On("world_update", handleEvents);
        _socket.On("sessionEnded", handleSessionEnd);
    }

    public void Dispose()
    {
        _socket.Off("world_update", handleEvents);
        _socket.Off("sessionEnded", handleSessionEnd);
    }
}

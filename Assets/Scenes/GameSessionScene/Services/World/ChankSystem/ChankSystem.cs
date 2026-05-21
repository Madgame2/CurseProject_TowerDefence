using Common.Services.Net.Modules;
using Common.systems.MainThread;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

public class ChankSystem : MonoBehaviour
{
    [SerializeField] private Transform _worldRoot;
    [Inject] private WebSocketModule _socket;
    [Inject] private MainThreadDispatcher _mainThread;
    [Inject] private DecorationManager _decorationSystem;

    [SerializeField] private GameObject _ChankPrefab;

    private Dictionary<Vector2, Chank> _chanks = new();

    ConcurrentQueue<ChankMetaData> queue = new();
    private bool _isInited = false;
    private bool _isSubscribed;

    private void Start()
    {
        Clear();
        _socket.onConnect += InitSubscrationsToEvents;
    }

    private void OnDisable()
    {
        _isInited = false;
        _socket.onConnect -= InitSubscrationsToEvents;
    }


    private void OnDestroy()
    {
        _isInited = false;
        _isSubscribed = false;
        _socket.onConnect -= InitSubscrationsToEvents;


        _chanks.Clear();
        _socket.Off("chankPreload", handlePreloadedChank);
    }



    public void InitSubscrationsToEvents()
    {
        if (_isInited) return;

        if (_isSubscribed) return;

        _socket.On("chankPreload", handlePreloadedChank);
        _isSubscribed = true;
        _isInited = true;
    }

    private async Task HandlePlayersMetaData(string arg)
    {
        throw new NotImplementedException();
    }

    private void handlePreloadedChank(string arg)
    {
        var chankData = JsonConvert.DeserializeObject<ChankMetaData>(arg);

        _ = _socket.Send("chankApply", new { });

        _mainThread.Run(() =>
        {
            var obj = Instantiate(_ChankPrefab, _worldRoot);
            obj.transform.position = new Vector3(chankData.x * 160, 0, chankData.z * 160);

            if(obj.TryGetComponent<Chank>(out Chank chank))
            {
                chank.Position = new Vector2(chankData.x,chankData.z);
                if (_chanks.ContainsKey(chank.Position))
                    return;

                _chanks[chank.Position] = chank;
            }
        });
    }

    public void Clear()
    {
        _chanks.Clear();
        queue.Clear();
        _isInited = false;
    }
    internal void handleChankUpdate(ChankUpdate message)
    {
        Vector2 chankPos = new Vector2(message.chankPos.X, message.chankPos.Y);
         if( _chanks.TryGetValue(chankPos, out Chank chank))
        {

            _decorationSystem.PlaceDecorationAt(chank, message.chankCell, message.cellData);
        }
    }
   

}

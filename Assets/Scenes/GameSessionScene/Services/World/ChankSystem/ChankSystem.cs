using Common.Services.Net.Modules;
using Common.systems.MainThread;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class ChankSystem : MonoBehaviour
{
    [SerializeField] private Transform _worldRoot;
    [Inject] private WebSocketModule _socket;
    [Inject] private MainThreadDispatcher _mainThread;

    [SerializeField] private GameObject _debugPlane;


    private bool _isInited = false;

    private void Start()
    {
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
        _socket.onConnect -= InitSubscrationsToEvents;
    }

    public void InitSubscrationsToEvents()
    {
        if (!_isInited)
        {
            _socket.On("chankPreload", handlePreloadedChank);
            _isInited = true;
        }
    }

    private async Task HandlePlayersMetaData(string arg)
    {
        throw new NotImplementedException();
    }

    private async Task handlePreloadedChank(string arg)
    {
         ChankMetaData chankData = JsonConvert.DeserializeObject<ChankMetaData>(arg);
        
        _mainThread.Run(() =>
        {
            Debug.Log($"{chankData.x}: {chankData.z}");
            GameObject obj = Instantiate(_debugPlane, _worldRoot, false);
            obj.transform.position= new Vector3(chankData.x * 160, 0, chankData.z * 160);
        });

        _ = _socket.Send("chankApply", new { });
    }
}

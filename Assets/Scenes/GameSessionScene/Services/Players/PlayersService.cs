using Common.Services.Net.Modules;
using Common.systems.MainThread;
using Common.systems.ProfileSystem;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlayersService: IDisposable
{
    private readonly WebSocketModule _socket;
    private readonly PlayerStorage _playersStorage;
    [Inject] private readonly MainThreadDispatcher _mainThread;
    [Inject] private ProfileManager _profileManager;
    [Inject] private ThirdPersonCamera _cameraController;


    public PlayersService(
        WebSocketModule socket,
        PlayerStorage playersStorage)
    {
        _socket = socket;
        _playersStorage = playersStorage;
    }
    public void Dispose()
    {
        _socket.Off("playersInit_metadata", handleInitNewPlayer);
    }

    public void Init()
    {
        _socket.On("playersInit_metadata", handleInitNewPlayer);
    }


    private async Task handleInitNewPlayer(string arg)
    {
        var players = JsonConvert.DeserializeObject<NewPlayerRequestDto[]>(arg);

        foreach (var player in players)
        {
            var dtoPosition = player.position;
            Vector3 positition = new Vector3(
                dtoPosition.X,
                dtoPosition.Y,
                dtoPosition.Z
                );

            var dtoRotation = player.rotation;
            Vector3 rotation = new Vector3(
                dtoRotation.X,
                dtoRotation.Y,
                dtoRotation.Y
                );

            _mainThread.Run(() =>
            {
                _playersStorage.AddPlayer(player.playerId, "prefab", player.hp, positition, rotation);
            });
        }

        _mainThread.Run(() =>
        {
            LinkCameraToPlayer();
        });
        

        _ = _socket.Send("meataDataApply", new { });
    }

    public void LinkCameraToPlayer()
    {
        GameObject playerObject = _playersStorage.GetByPlayerGameObjectId(_profileManager.Profile.UserId);
        _cameraController.SetTarget(playerObject.transform);
    }
}

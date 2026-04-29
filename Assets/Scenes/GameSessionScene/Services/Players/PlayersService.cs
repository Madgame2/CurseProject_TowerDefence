using Common.Services.Net.Modules;
using Common.systems.MainThread;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class PlayersService: IDisposable
{
    private readonly WebSocketModule _socket;
    private readonly PlayerStorage _playersStorage;
    [Inject] private readonly MainThreadDispatcher _mainThread;

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


        _ = _socket.Send("meataDataApply", new { });
    }
}

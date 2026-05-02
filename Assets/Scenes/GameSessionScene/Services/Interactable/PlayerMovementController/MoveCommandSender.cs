using Common.Services.Net.Modules;
using Common.systems.ProfileSystem;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MoveCommandSender
{
    [Inject] private WebSocketModule _socket;
    [Inject] ProfileManager _profilemanager;

    internal async Task sendMoveToPossition(Vector3 mousePos)
    {
        var payload = new MoveRequest
        {
            PlayerID = _profilemanager.Profile.UserId,
            X = mousePos.x/10,
            Z = mousePos.z/10
        };

        Debug.Log("Sended");
        await _socket.SendRequest("MoveTo", payload);
    }
}


public class MoveRequest
{
    public string PlayerID;
    public float X { get; set; }
    public float Z { get; set; }
}

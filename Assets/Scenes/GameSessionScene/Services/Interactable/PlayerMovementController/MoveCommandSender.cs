using Common.Services.Net.Modules;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MoveCommandSender
{
    [Inject] private WebSocketModule _socket;

    internal async Task sendMoveToPossition(Vector3 mousePos)
    {
        var payload = new MoveRequest
        {
            X = mousePos.x,
            Z = mousePos.z
        };

        Debug.Log("Sended");
        await _socket.SendRequest("MoveTo", payload);
    }
}


public class MoveRequest
{
    public float X { get; set; }
    public float Z { get; set; }
}

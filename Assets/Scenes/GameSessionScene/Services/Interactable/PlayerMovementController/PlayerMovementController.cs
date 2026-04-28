using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerMovementController
{
    [Inject] private MoveCommandSender _sender;


    private GameObject debugPointer;

    internal void MoveTo(Transform player, Vector3 mousePos)
    {
        SpawnDebugPointer(mousePos);

        _ = _sender.sendMoveToPossition(mousePos);
    }

    private void SpawnDebugPointer(Vector3 pos)
    {
        if (debugPointer == null)
        {
            debugPointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            debugPointer.transform.localScale = new Vector3(10, 10, 10);
        }

        debugPointer.transform.position = pos;
    }
}
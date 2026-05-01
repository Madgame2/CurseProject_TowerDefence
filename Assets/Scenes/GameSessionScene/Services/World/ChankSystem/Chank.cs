using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Chank : MonoBehaviour
{
    //[Inject] private DecorationManager _decorationManager;


    public Vector2 Position;
    public Dictionary<Vector2, long> EntiersRecords = new();


    public void InitChank()
    {
        //_decorationManager.LoadChankDecorations(this);
    }

    private void OnDisable()
    {
        //_decorationManager.UnloadChankDecorations(this);
    }
}

using System;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using Common.Services.Net.Modules;


public class BuildSystem : MonoBehaviour
{
    [SerializeField] private BuildObjectDatabase _objectsDataBase;
    [SerializeField] private float cellSize = 10f;

    [Inject] private WebSocketModule _socket;
    [Inject] private DiContainer _container;

    private Dictionary<BuildObjectsEnum, GameObject> _buildObjectsPool = new();
    private GameObject CurrentPreview = null;


    public void Start()
    {
        foreach (var obj in _objectsDataBase.buildObjects)
        {
            if (obj.previewPrefab == null) continue;

            GameObject newObject =  _container.InstantiatePrefab(obj.previewPrefab);
            newObject.SetActive(false);

            _buildObjectsPool.Add(obj.type, newObject);
        }
    }


    internal void UpdatePreview(Vector3 worldPos, BuildObjectsEnum objType)
    {
        if(CurrentPreview != null)
        {
            CurrentPreview.SetActive(false);
            CurrentPreview = _buildObjectsPool[objType];
        }
        CurrentPreview = _buildObjectsPool[objType];
        CurrentPreview.SetActive(true);
        UpdatePosition(worldPos);
    }

    private void UpdatePosition(Vector3 worldPos)
    {
        float x = Mathf.Floor(worldPos.x / cellSize) * cellSize + cellSize / 2f;
        float z = Mathf.Floor(worldPos.z / cellSize) * cellSize + cellSize / 2f;

        CurrentPreview.transform.position = new Vector3(x, worldPos.y, z);
    }


    public void PlaceRequestForBuilding(Vector3 worldPos, BuildObjectsEnum objType)
    {
        float x = Mathf.Floor(worldPos.x / cellSize) * cellSize + cellSize / 2f;
        float z = Mathf.Floor(worldPos.z / cellSize) * cellSize + cellSize / 2f;

        var BuildConfig = _objectsDataBase.GetByType(objType);
        var request = new RequestForBuilding();

        request.buildNetID = BuildConfig.NetID;
        request.worldX = (int)Math.Floor(x / 10);
        request.worldZ = (int)(Math.Floor(z / 10));

        CurrentPreview.SetActive(false);


        _ = _socket.Send("BuildObject", request);
    }
}

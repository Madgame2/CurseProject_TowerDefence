using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DecorationManager : MonoBehaviour
{
    private Dictionary<Chank, List<DecorationInstance>> chunkToDecorations = new();
    [SerializeField] private DecorationConfigDatabase config;
    [SerializeField] private DecorationFactory factory;
    [SerializeField] private EntityManager entityManager;

    [Inject] private DiContainer _container;
    private Dictionary<Vector2, GameObject> Placed_Decorations = new();

    public void Start()
    {
        config.Init();
    }


    public void LoadChankDecorations(Chank chank)
    {
        if (!chunkToDecorations.TryGetValue(chank, out var decorations))
            return;

        foreach( var objects in chank.EntiersRecords)
        {
            foreach (var deco in decorations)
            {
                if (!deco.loadedInChunks.Add(chank))
                    continue;



                if (!deco.IsLoaded)
                {
                    deco.gameObject = factory.Create(deco);
                }
            }
        }
    }

    public void UnloadChankDecorations(Chank chank)
    {
        if (!chunkToDecorations.TryGetValue(chank, out var decorations))
            return;

        foreach (var deco in decorations)
        {
            // если чанка не было — пропускаем
            if (!deco.loadedInChunks.Remove(chank))
                continue;

            if (deco.loadedInChunks.Count == 0)
            {
                factory.Destroy(deco);
                deco.gameObject = null;
            }
        }
    }

    internal void PlaceDecorationAt(Chank chank, Vector2Dto chankCell, int cellData)
    {
        Vector3 localPos = new Vector3(chankCell.X, 0, chankCell.Y);
        Vector3 globalPos = chank.transform.position + localPos * 10;

        Vector2 _global2dpos = new Vector2(globalPos.x, globalPos.z);
        if(Placed_Decorations.TryGetValue(_global2dpos, out GameObject gameobjec))
        {
            Destroy(gameobjec);
        }
        if (cellData == 0) return;

        GameObject decorationPrefab = config.Get(cellData)?.prefab;
        if (decorationPrefab == null) return;

        GameObject spawnedDecorations = _container.InstantiatePrefab(decorationPrefab);
        spawnedDecorations.transform.parent = chank.transform;
        spawnedDecorations.transform.position = globalPos;

        Placed_Decorations.Add(_global2dpos, spawnedDecorations);
    }

    private void OnDestroy()
    {
        Placed_Decorations.Clear();
        chunkToDecorations.Clear();
    }
}
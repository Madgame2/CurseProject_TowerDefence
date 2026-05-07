using Common.Services.Net.Modules;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NpcManager : MonoBehaviour
{
    [SerializeField] private NpcDataBase _npcDataBase;
    [SerializeField] private GameObject _enemySpawnVFX;

    [Inject] private DiContainer _container;
    [Inject] private IVfxService _VFX;

    private Dictionary<string, GameObject> _spawnedNpcs = new();
    private NpcFactory _factory;


    private void Awake()
    {
        _factory = new NpcFactory(_npcDataBase, _container);
    }

    public void HandleNpcUpdate(NpcEvent npc)
    {
        switch (npc.enventType)
        {
            case NpcEventType.SPAWN:
                {
                    var data = npc.data.ToObject<NpcSpawnData>();
                    if(data!=null)
                    {
                        SpawnNewNpc(npc, data);
                    }
                    else
                    {
                        Debug.LogError("Invalid data for SPAWN");
                    }
                    break;
                }

            case NpcEventType.TERMINATE:
                {
                    TerminateNpc(npc);
                    break;
                }

            case NpcEventType.UPDATE:
                {
                    var data = npc.data.ToObject<NpcUpdateData>();
                    if (data!=null)
                    {
                        UpdateNpc(npc, data);
                    }
                    else
                    {
                        Debug.LogError("Invalid data for UPDATE");
                    }
                    break;
                }
        }
    }
    private Vector3 ConvertPosition(Vector2Dto pos)
    {
        return new Vector3(pos.X, 0, pos.Y);
    }

    private void TerminateNpc(NpcEvent npc)
    {
        if (!_spawnedNpcs.TryGetValue(npc.npcId, out var go))
        {
            Debug.LogWarning($"NPC with id {npc.npcId} not found");
            return;
        }

        Destroy(go);
        _spawnedNpcs.Remove(npc.npcId);
    }

    private void SpawnNewNpc(NpcEvent npc, NpcSpawnData data)
    {
        if (_spawnedNpcs.ContainsKey(npc.npcId))
        {
            Debug.LogWarning($"NPC with id {npc.npcId} already exists");
            return;
        }

        Vector3 position = ConvertPosition(data.position);

        var go = _factory.Create(npc.npcType, position);

        _spawnedNpcs.Add(npc.npcId, go);

        if(data.behaver== Npcbehavior.ENEMY)
        {
            _VFX.PlayEffect(_enemySpawnVFX, go.transform.position);
        }
    }

    private void UpdateNpc(NpcEvent npc, NpcUpdateData data)
    {
        if (!_spawnedNpcs.TryGetValue(npc.npcId, out var go))
        {
            Debug.LogWarning($"NPC with id {npc.npcId} not found");
            return;
        }

        Vector3 position = ConvertPosition(data.position);

        go.transform.position = position;
    }

    public class NpcFactory
    {
        private readonly NpcDataBase _dataBase;
        private readonly DiContainer _container;

        public NpcFactory(NpcDataBase dataBase, DiContainer container)
        {
            _dataBase = dataBase;
            _container = container;
        }

        public GameObject Create(NpcTypes type, Vector3 position)
        {
            var prefab = _dataBase.GetPrefab(type);

            if (prefab == null)
            {
                Debug.LogError($"Prefab for {type} not found!");
                return null;
            }

            GameObject spawnedObject = _container.InstantiatePrefab(prefab);
            spawnedObject.transform.position = position;

            return spawnedObject;
        }
    }


}

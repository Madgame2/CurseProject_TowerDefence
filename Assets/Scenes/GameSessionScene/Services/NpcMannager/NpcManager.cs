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
    public Dictionary<string, NpcState> _npcStates = new();
    private NpcFactory _factory;


    [SerializeField] private float positionSmooth = 10f;
    [SerializeField] private float rotationSmooth = 10f;
    [SerializeField] private float snapDistance = 5f;


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
                    Debug.Log($"Пришло обновление {npc.npcId}");
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
        return new Vector3(pos.X*10, 0, pos.Y*10);
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
        _npcStates.Remove(npc.npcId);
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


        var npcState = new NpcState();
        npcState.targetPosition = position;

        _spawnedNpcs.Add(npc.npcId, go);
        _npcStates.Add(npc.npcId, npcState);

        if (data.behaver== Npcbehavior.ENEMY)
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

        switch (data.dataType)
        {
            case DataType.NPC_STATE:
                {
                    var state = _npcStates[npc.npcId];
                    Vector3 position = ConvertPosition(data.position);


                    if (state != null)
                    {
                        state.targetPosition = position;

                        state.targetRotation = Quaternion.Euler(
                            data.rotation.X,
                            data.rotation.Y,
                            data.rotation.Z
                        );

                        if (_spawnedNpcs[npc.npcId].TryGetComponent<SkeletonAnimController>(out SkeletonAnimController controller))
                        {
                            controller.SetVelocity(new Vector3(data.velocity.X, 0, data.velocity.Y));
                        }
                    }
                }
                break;

            case DataType.ACTION:
                {
                    switch (data.action)
                    {
                        case ActionTypes.ATTACK:
                            if (_spawnedNpcs[npc.npcId].TryGetComponent<SkeletonAnimController>(out SkeletonAnimController controller))
                            {
                                controller.PlayAttack();
                            }
                            break;
                    }
                }
                break;
        }

    }


    private void Update()
    {
        foreach (var kvp in _npcStates)
        {
            var npcId = kvp.Key;
            var state = kvp.Value;

            var npc = _spawnedNpcs.GetValueOrDefault(npcId);
            if (npc == null)
                continue;

            Transform npcTransform = npc.transform;

            // snap check
            if (Vector3.Distance(npcTransform.position, state.targetPosition) > snapDistance)
            {
                npcTransform.position = state.targetPosition;
            }
            else
            {
                npcTransform.position = Vector3.Lerp(
                    npcTransform.position,
                    state.targetPosition,
                    Time.deltaTime * positionSmooth
                );
            }

            npcTransform.rotation = Quaternion.Slerp(
                npcTransform.rotation,
                state.targetRotation,
                Time.deltaTime * rotationSmooth
            );
        }
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

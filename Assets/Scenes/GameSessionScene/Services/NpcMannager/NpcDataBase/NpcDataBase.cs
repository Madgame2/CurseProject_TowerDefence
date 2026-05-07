using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcDataBase", menuName = "World/NpcDataBase")]
public class NpcDataBase : ScriptableObject
{
    [SerializeField] private List<NpcDataBaseRecord> records;

    private Dictionary<NpcTypes, GameObject> _lookup;


    private void Init()
    {
        if (_lookup != null) return;

        _lookup = new Dictionary<NpcTypes, GameObject>();

        foreach (var record in records)
        {
            if (_lookup.ContainsKey(record.npcType))
            {
                Debug.LogWarning($"Duplicate NPC type in database: {record.npcType}");
                continue;
            }

            _lookup.Add(record.npcType, record.prefab);
        }
    }


    public GameObject GetPrefab(NpcTypes type)
    {
        Init();

        if (_lookup.TryGetValue(type, out var prefab))
        {
            return prefab;
        }

        Debug.LogError($"NpcDataBase: prefab for type {type} not found!");
        return null;
    }
}

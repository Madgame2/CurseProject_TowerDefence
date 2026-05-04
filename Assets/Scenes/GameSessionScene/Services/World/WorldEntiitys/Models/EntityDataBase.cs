using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityDataBase", menuName = "Game/Entity DataBase")]
public class EntityDataBase_enum : ScriptableObject
{
    [SerializeField] private List<EntityConfig> configs;

    private Dictionary<EntityesEnum, EntityConfig> _map;

    private void OnEnable()
    {
        BuildMap();
    }

    private void BuildMap()
    {
        _map = configs
            .Where(c => c != null)
            .GroupBy(c => c.type)
            .ToDictionary(g => g.Key, g => g.First());
    }

    public EntityConfig Get(EntityesEnum type)
    {
        if (_map == null)
            BuildMap();

        if (!_map.TryGetValue(type, out var config))
        {
            Debug.LogError($"❌ No config for entity type: {type}");
            return null;
        }

        return config;
    }
}

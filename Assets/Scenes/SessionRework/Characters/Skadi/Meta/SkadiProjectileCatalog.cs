using System;
using System.Collections.Generic;
using Scenes.SessionRework.Characters.Base.Configs;
using Scenes.SessionRework.Characters.Meta;
using Scenes.SessionRework.Characters.Skadi.Meta;
using UnityEngine;

namespace Scenes.SessionRework.Characters.Skadi.Abilities.Configs
{
    [CreateAssetMenu(menuName = "Game/Characters/Skadi/Projectiles")]
    public class SkadiProjectileCatalog: CharacterProjectileCatalog
    {
        public override string CharacterKey => "Skadi";
        
        [SerializeField]
        private List<Entry> _entries;
        
        public override IEnumerable<ProjectileDefinition> GetDefinitions()
        {
            foreach (var entry in _entries)
            {
                yield return new ProjectileDefinition(
                    entry.NetworkId,
                    CharacterKey,
                    entry.Type.ToString(),
                    entry.Prefab);
            }
        }
        
        public bool TryGetPrefab(
            SkadiProjectileType type,
            out GameObject prefab)
        {
            var entry = _entries.Find(x => x.Type == type);

            prefab = entry?.Prefab;

            return prefab != null;
        }
        
        [Serializable]
        public class Entry
        {
            public SkadiProjectileType Type;
            [HideInInspector]
            public uint NetworkId;
            public GameObject Prefab;
        }
    }
}
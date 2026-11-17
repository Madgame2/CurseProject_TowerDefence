using UnityEngine;

namespace Scenes.SessionRework.Characters.Meta
{
    public readonly struct ProjectileDefinition
    {
        public readonly uint NetworkId;
        public readonly string CharacterKey;
        public readonly string SymbolicName;
        public readonly GameObject Prefab;

        public ProjectileDefinition(
            uint networkId,
            string characterKey,
            string symbolicName,
            GameObject prefab)
        {
            NetworkId = networkId;
            CharacterKey = characterKey;
            SymbolicName = symbolicName;
            Prefab = prefab;
        }
    }
}
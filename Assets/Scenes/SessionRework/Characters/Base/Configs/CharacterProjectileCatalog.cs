using System.Collections.Generic;
using Scenes.SessionRework.Characters.Meta;
using UnityEngine;

namespace Scenes.SessionRework.Characters.Base.Configs
{
    public abstract class CharacterProjectileCatalog : ScriptableObject
    {
        public abstract string CharacterKey { get; }
        
        public abstract IEnumerable<ProjectileDefinition> GetDefinitions();
    }
}
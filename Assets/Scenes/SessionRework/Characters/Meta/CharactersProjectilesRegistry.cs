using System.Collections.Generic;
using Scenes.SessionRework.Characters.Base.Configs;
using UnityEngine;

namespace Scenes.SessionRework.Characters.Meta
{
    [CreateAssetMenu(menuName = "Game/Characters/CommonProjectilesRegistry")]
    public  class CharactersProjectilesRegistry:  ScriptableObject
    {
        [SerializeField]
        private List<CharacterProjectileCatalog> _charactersConfigs;
    }
}
using Editor.Interfaces;
using Scenes.SessionRework.Scripts.GameWorld.Graph.BiomGraph.Meta.Enums;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.Core.Meta
{
    [CreateAssetMenu(fileName = "NewBiomeConfig", menuName = "Terrain/Biome Texture Config")]
    public class BiomeTextureConfig:ScriptableObject, ITextureProvider
    {
        public Texture2D GrassTexture;
        public Texture2D StoneTexture;
        
        public Texture2D[] GetTextures()
        {
            return new [] { GrassTexture, StoneTexture };
        }
        
        [System.Serializable]
        private struct BiomeTextureInfo
        {
            public BiomeType  BiomeType;
            public Texture2D BiomeTexture;
        }
    }
}
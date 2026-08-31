using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.World
{
    public struct ChunkStorageComponent : IComponent
    {
        public HashSet<Vector2Int> ChunkStorage; 
    }
}
using Scenes.SessionRework.Scripts.GameWorld.Model;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.Interfaces
{
    public interface IChunkReader
    {
        void Link(ref Chunk chunkModel);
        void Clear();
        
        Transform Transform { get; }
        GameObject GameObject { get;  }
        
    }
}
using Scenes.SessionRework.Scripts.World.Model;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.World.Interfaces
{
    public interface IChunkReader
    {
        void Link(ref Chunk chunkModel);
        void Clear();
        
        Transform Transform { get; }
        GameObject GameObject { get;  }
        
    }
}
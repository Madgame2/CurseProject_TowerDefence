using System;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.GameWorld.Interfaces
{
    public interface INodeDirectory
    {
        bool TryGetValue(NodeType type, out Type nodeType);
    }
}
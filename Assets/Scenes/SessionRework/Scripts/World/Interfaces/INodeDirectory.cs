using System;
using Scenes.SessionRework.Scripts.Services.Sync.DTO.WorldGenerationGraph.Meta;

namespace Scenes.SessionRework.Scripts.World.Interfaces
{
    public interface INodeDirectory
    {
        bool TryGetValue(NodeType type, out Type nodeType);
    }
}
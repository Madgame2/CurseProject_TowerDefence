using Scenes.SessionRework.Scripts.Network.Meta;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Tools.NetObjectSerializer.Interfaces
{
    public interface INetObjectSerializer
    {
        void Serialize(NetObjectDefinition prefab, string outputPath);
    }
}
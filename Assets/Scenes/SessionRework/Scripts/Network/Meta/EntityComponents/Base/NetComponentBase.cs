using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Base.DTO;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Base
{
    public abstract class NetComponentBase : MonoBehaviour
    {
        public virtual int SerializationVersion => 1;

        public abstract object GetSerializationData();

        public virtual object GetSerializationMetadata()
        {
            return new NetComponentMetadataDto
            {
                Version = SerializationVersion
            };
        }
    }
}
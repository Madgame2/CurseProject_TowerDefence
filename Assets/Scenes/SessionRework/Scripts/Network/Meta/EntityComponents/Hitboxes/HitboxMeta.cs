using System;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Base;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Enums;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model.Base;
using Scenes.SessionRework.Scripts.Network.Tools.NetObjectSerializer;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes
{
    public class HitboxMeta: NetComponentBase
    {
        [SerializeField]
        private HitboxShape _shape;

        [SerializeField]
        private HitboxShapes _data;
        
        
        public HitboxShape Shape => _shape;
        public HitboxShapes Data => _data;
        public override object GetSerializationData()
        {
            return _shape switch
            {
                HitboxShape.Sphere => _data.Sphere,

                HitboxShape.Capsule => _data.Capsule,

                HitboxShape.Box => _data.Box,

                _ => throw new NotSupportedException(
                    $"Unsupported hitbox shape: {_shape}")
            };
        }
        
        public override object GetSerializationMetadata()
        {
            return new HitboxMetadataDto
            {
                ShapeType = _shape.ToString(),
                Version = 1
            };
        }
    }
}
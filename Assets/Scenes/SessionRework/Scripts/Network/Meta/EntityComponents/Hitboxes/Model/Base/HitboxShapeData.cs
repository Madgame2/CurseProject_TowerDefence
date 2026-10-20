using System;
using System.Xml.Serialization;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Enums;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model.Base
{
    [Serializable]
    [XmlInclude(typeof(SphereHitboxData))]
    [XmlInclude(typeof(CapsuleHitboxData))]
    [XmlInclude(typeof(BoxHitboxData))]
    public abstract class HitboxShapeData
    {
        public abstract HitboxShape HitboxShape { get; }

        public Vector3 Center = Vector3.zero;
        public Quaternion Rotation = Quaternion.identity;
    }
}
using System;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Enums;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model.Base;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model
{
    [Serializable]
    public class SphereHitboxData : HitboxShapeData
    {
        public override HitboxShape HitboxShape => HitboxShape.Sphere;

        
        public float Radius = 0.5f;
    }
}
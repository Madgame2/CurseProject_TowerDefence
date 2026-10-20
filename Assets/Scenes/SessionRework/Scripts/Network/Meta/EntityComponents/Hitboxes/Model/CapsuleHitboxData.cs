using System;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Enums;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model.Base;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model
{
    [Serializable]
    public class CapsuleHitboxData : HitboxShapeData
    {
        public override HitboxShape HitboxShape => HitboxShape.Capsule;
        
        public float Radius = 0.5f;
        public float Height = 2f;
    }
}
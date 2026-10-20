using System;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Enums;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model.Base;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model
{
    [Serializable]
    public class BoxHitboxData : HitboxShapeData
    {
        public Vector3 Size = Vector3.one;
        public override HitboxShape HitboxShape => HitboxShape.Box;
    }
}
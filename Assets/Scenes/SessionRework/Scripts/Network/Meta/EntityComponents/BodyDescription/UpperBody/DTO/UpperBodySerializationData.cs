using System;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents
{
    [Serializable]
    public struct UpperBodySerializationData
    {
        public Vector3 PivotLocalPosition;
        public Vector3 AimOriginOffsetFromPivot;
        public Vector3 AimForwardLocal;
    }
}
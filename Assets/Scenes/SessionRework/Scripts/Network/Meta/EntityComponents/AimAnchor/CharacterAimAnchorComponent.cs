using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.AimAnchor.DTO;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Base;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.AimAnchor
{

    using UnityEngine;

    public class CharacterAimAnchorComponent : NetComponentBase
    {
        [Header("Attachment")]
        [SerializeField]
        private UpperBodyComponent _parent;

        [SerializeField]
        private Vector3 _localOffset;

        [Header("Gizmos")]
        [SerializeField, Min(0.01f)]
        private float _sphereRadius = 0.07f;

        [SerializeField, Min(0.01f)]
        private float _directionLength = 0.5f;

        public UpperBodyComponent Parent => _parent;

        public Vector3 LocalOffset => _localOffset;

        public Vector3 WorldPosition =>
            _parent != null
                ? _parent.TransformPoint(_localOffset)
                : transform.TransformPoint(_localOffset);

        public override object GetSerializationData()
        {
            return new CharacterAimAnchorData
            {
                LocalOffset = _localOffset
            };
        }

        private void OnDrawGizmos()
        {
            Vector3 position = WorldPosition;

            Quaternion rotation = _parent != null
                ? _parent.Rotation
                : transform.rotation;

            // Точка прицеливания
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, _sphereRadius);
            Gizmos.DrawSphere(position, _sphereRadius * 0.35f);

            // Локальное направление взгляда
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(
                position,
                rotation * Vector3.forward * _directionLength
            );

            // Связь с pivot верхней части тела
            if (_parent != null)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(
                    _parent.RotationPivot,
                    position
                );
            }
        }

        private void OnValidate()
        {
            _sphereRadius = Mathf.Max(0.01f, _sphereRadius);
            _directionLength = Mathf.Max(0.01f, _directionLength);
        }
    }

    [System.Serializable]
    public struct CharacterAimAnchorData
    {
        public Vector3 LocalOffset;
    }

}
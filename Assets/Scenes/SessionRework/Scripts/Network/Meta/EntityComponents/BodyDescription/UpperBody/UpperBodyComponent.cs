using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Base;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents
{
    using System;
    using UnityEngine;
    using System;
    using UnityEngine;

    public class UpperBodyComponent : NetComponentBase
    {
        [Header("Capsule Visualization")] [SerializeField]
        private float _centerOffset = 0.5f;

        [SerializeField] private float _radius = 0.25f;

        [SerializeField] private float _height = 1.2f;

        [Header("Rotation Preview")] [SerializeField]
        private Vector3 _rotation;

        [Header("Bake Transforms")] [SerializeField]
        private Transform _rotationPivotTransform;

        [SerializeField] private Transform _aimOriginTransform;

        [SerializeField] private Transform _lookAtTargetTransform;

        [Header("Gizmos")] [SerializeField]
        private float _pivotSphereRadius = 0.08f;

        [SerializeField] private float _aimSphereRadius = 0.06f;

        [SerializeField] private float _targetSphereRadius = 0.06f;

        [SerializeField] private float _forwardLength = 0.75f;

        // Положение центра вращения в мировых координатах.
        public Vector3 RotationPivot =>
            _rotationPivotTransform != null
                ? _rotationPivotTransform.position
                : transform.position;

        // Виртуальное вращение UpperBody.
        // В нейтральном состоянии оно совпадает с вращением PlayerRoot.
        public Quaternion Rotation =>
            transform.rotation * Quaternion.Euler(_rotation);

        // Позиция точки прицеливания с учетом Preview Rotation.
        public Vector3 AimOriginPosition
        {
            get
            {
                if (_aimOriginTransform == null ||
                    _rotationPivotTransform == null)
                    return Vector3.zero;

                Vector3 pivotLocal =
                    transform.InverseTransformPoint(
                        _rotationPivotTransform.position);

                Vector3 aimLocal =
                    transform.InverseTransformPoint(
                        _aimOriginTransform.position);

                Vector3 offset = aimLocal - pivotLocal;

                return RotationPivot + Rotation * offset;
            }
        }

        // Преобразование локальной точки относительно Pivot.
        public Vector3 TransformPoint(Vector3 localPoint)
        {
            return RotationPivot + Rotation * localPoint;
        }

        public override object GetSerializationData()
        {
            ValidateBakeTransforms();

            // Все координаты переводим в систему PlayerRoot.

            Vector3 pivotLocal =
                transform.InverseTransformPoint(
                    _rotationPivotTransform.position);

            Vector3 aimOriginLocal =
                transform.InverseTransformPoint(
                    _aimOriginTransform.position);

            // Направление от точки прицеливания к контрольной точке.
            Vector3 aimDirection =
                _lookAtTargetTransform.position -
                _aimOriginTransform.position;

            if (aimDirection.sqrMagnitude < 0.000001f)
            {
                throw new InvalidOperationException(
                    "LookAtTargetTransform must differ from AimOriginTransform.");
            }

            aimDirection.Normalize();

            Vector3 aimForwardLocal =
                transform.InverseTransformDirection(aimDirection).normalized;

            return new UpperBodySerializationData
            {
                PivotLocalPosition = pivotLocal,

                // Смещение точки прицеливания относительно Pivot.
                // В координатах PlayerRoot.
                AimOriginOffsetFromPivot = aimOriginLocal - pivotLocal,

                // Базовое направление прицеливания.
                AimForwardLocal = aimForwardLocal
            };
        }

        private void ValidateBakeTransforms()
        {
            if (_rotationPivotTransform == null)
                throw new InvalidOperationException(
                    "UpperBody: RotationPivotTransform is not assigned.");

            if (_aimOriginTransform == null)
                throw new InvalidOperationException(
                    "UpperBody: AimOriginTransform is not assigned.");

            if (_lookAtTargetTransform == null)
                throw new InvalidOperationException(
                    "UpperBody: LookAtTargetTransform is not assigned.");
        }

        private void OnValidate()
        {
            _radius = Mathf.Max(0.01f, _radius);
            _height = Mathf.Max(_radius * 2f, _height);

            _pivotSphereRadius = Mathf.Max(0.01f, _pivotSphereRadius);
            _aimSphereRadius = Mathf.Max(0.01f, _aimSphereRadius);
            _targetSphereRadius = Mathf.Max(0.01f, _targetSphereRadius);
        }

        private void OnDrawGizmos()
        {
            if (_rotationPivotTransform == null)
                return;

            DrawUpperBodyGizmos();
            DrawAimGizmos();
        }

        private void DrawUpperBodyGizmos()
        {
            Vector3 pivot = RotationPivot;

            Quaternion rotation = Rotation;

            // Rotation Pivot.
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                pivot,
                _pivotSphereRadius
            );

            // Capsule center.
            Vector3 capsuleCenter =
                pivot + rotation * (Vector3.up * _centerOffset);

            float halfHeight = Mathf.Max(
                0f,
                _height * 0.5f - _radius
            );

            Vector3 up = rotation * Vector3.up;
            Vector3 right = rotation * Vector3.right;
            Vector3 forward = rotation * Vector3.forward;

            Vector3 top = capsuleCenter + up * halfHeight;
            Vector3 bottom = capsuleCenter - up * halfHeight;

            // Capsule.
            Gizmos.color = Color.cyan;

            Gizmos.DrawWireSphere(top, _radius);
            Gizmos.DrawWireSphere(bottom, _radius);

            Gizmos.DrawLine(
                top + right * _radius,
                bottom + right * _radius
            );

            Gizmos.DrawLine(
                top - right * _radius,
                bottom - right * _radius
            );

            Gizmos.DrawLine(
                top + forward * _radius,
                bottom + forward * _radius
            );

            Gizmos.DrawLine(
                top - forward * _radius,
                bottom - forward * _radius
            );

            // Capsule axes.
            float axisLength = _height * 0.5f;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                capsuleCenter,
                capsuleCenter + right * axisLength
            );

            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                capsuleCenter,
                capsuleCenter + up * axisLength
            );

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                capsuleCenter,
                capsuleCenter + forward * axisLength
            );

            // Pivot to capsule center.
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pivot, capsuleCenter);
        }

        private void DrawAimGizmos()
        {
            if (_aimOriginTransform == null)
                return;

            Vector3 bakedAimOrigin =
                _aimOriginTransform.position;

            // Исходная точка из запекаемой позы.
            Gizmos.color = new Color(1f, 0.55f, 0.1f);

            Gizmos.DrawWireSphere(
                bakedAimOrigin,
                _aimSphereRadius
            );

            // Точка прицеливания с учетом Preview Rotation.
            Gizmos.color = Color.magenta;

            Gizmos.DrawSphere(
                AimOriginPosition,
                _aimSphereRadius * 0.55f
            );

            // Forward игрового корня.
            Gizmos.color = Color.blue;

            Gizmos.DrawRay(
                bakedAimOrigin,
                transform.forward * _forwardLength
            );

            if (_lookAtTargetTransform == null)
                return;

            Vector3 target = _lookAtTargetTransform.position;

            // Контрольная точка.
            Gizmos.color = Color.white;

            Gizmos.DrawWireSphere(
                target,
                _targetSphereRadius
            );

            // Базовое направление для запекания.
            Gizmos.color = Color.green;

            Gizmos.DrawLine(
                bakedAimOrigin,
                target
            );

            // Направление в текущем Preview Rotation.
            Gizmos.color = Color.cyan;

            Gizmos.DrawLine(
                AimOriginPosition,
                target
            );
        }
    }
}
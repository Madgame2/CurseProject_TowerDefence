using System;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Enums;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model.Base;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(HitboxMeta))]
    public class HitboxMetaEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty shape =
                serializedObject.FindProperty("_shape");

            SerializedProperty data =
                serializedObject.FindProperty("_data");

            EditorGUILayout.PropertyField(shape);

            switch ((HitboxShape)shape.enumValueIndex)
            {
                case HitboxShape.Sphere:
                    EditorGUILayout.PropertyField(
                        data.FindPropertyRelative("Sphere"));
                    break;

                case HitboxShape.Box:
                    EditorGUILayout.PropertyField(
                        data.FindPropertyRelative("Box"));
                    break;

                case HitboxShape.Capsule:
                    EditorGUILayout.PropertyField(
                        data.FindPropertyRelative("Capsule"));
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            HitboxMeta hitbox = (HitboxMeta)target;


            switch (hitbox.Shape)
            {
                case HitboxShape.Sphere:
                {
                    ProcessSphereShape(hitbox);
                }
                    break;
                case HitboxShape.Box:
                {
                    ProcessBoxShape(hitbox);
                }
                    break;
                case HitboxShape.Capsule:
                {
                    ProcessCapsuleShape(hitbox);
                }
                    break;
            }
        }

        private void ProcessSphereShape(HitboxMeta hitbox)
        {
            if (hitbox.Shape != HitboxShape.Sphere)
                return;

            SphereHitboxData data = hitbox.Data.Sphere;

            GetBaseTransforms(hitbox, data,
                out var center,
                out var rotation);
            
            Handles.color = Color.cyan;

            DrawSphere(center, rotation, data.Radius);
            
            EditorGUI.BeginChangeCheck();

            float newRadius = Handles.RadiusHandle(
                rotation,
                center,
                data.Radius
            );

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(hitbox, "Change Sphere Radius");

                data.Radius = Mathf.Max(
                    0.01f,
                    newRadius
                );

                EditorUtility.SetDirty(hitbox);
            }
        }

        private void DrawSphere(
            Vector3 center,
            Quaternion rotation,
            float radius)
        {
            Handles.color = Color.cyan;

            // XY
            Handles.DrawWireDisc(
                center,
                rotation * Vector3.forward,
                radius
            );

            // XZ
            Handles.DrawWireDisc(
                center,
                rotation * Vector3.up,
                radius
            );

            // YZ
            Handles.DrawWireDisc(
                center,
                rotation * Vector3.right,
                radius
            );
        }

        private void ProcessBoxShape(HitboxMeta hitbox)
        {
            if (hitbox.Shape != HitboxShape.Box)
                return;

            BoxHitboxData data = hitbox.Data.Box;

            GetBaseTransforms(hitbox, data,
                out var center,
                out var rotation);
            
            Handles.color = Color.cyan;

            DrawBox(center, rotation, data.Size);
            
            Vector3 right = rotation * Vector3.right;
            Vector3 up = rotation * Vector3.up;
            Vector3 forward = rotation * Vector3.forward;

            float handleSize = HandleUtility.GetHandleSize(center) * 0.1f;

// ========================
// X
// ========================

            Vector3 xPosition =
                center + right * (data.Size.x / 2f);

            EditorGUI.BeginChangeCheck();

            Vector3 newXPosition = Handles.Slider(
                xPosition,
                right,
                handleSize,
                Handles.CubeHandleCap,
                0f
            );

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(hitbox, "Change Box Size X");

                data.Size.x =
                    Mathf.Max(
                        0.01f,
                        Vector3.Distance(center, newXPosition) * 2f
                    );

                EditorUtility.SetDirty(hitbox);
            }


// ========================
// Y
// ========================

            Vector3 yPosition =
                center + up * (data.Size.y / 2f);

            EditorGUI.BeginChangeCheck();

            Vector3 newYPosition = Handles.Slider(
                yPosition,
                up,
                handleSize,
                Handles.CubeHandleCap,
                0f
            );

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(hitbox, "Change Box Size Y");

                data.Size.y =
                    Mathf.Max(
                        0.01f,
                        Vector3.Distance(center, newYPosition) * 2f
                    );

                EditorUtility.SetDirty(hitbox);
            }


// ========================
// Z
// ========================

            Vector3 zPosition =
                center + forward * (data.Size.z / 2f);

            EditorGUI.BeginChangeCheck();

            Vector3 newZPosition = Handles.Slider(
                zPosition,
                forward,
                handleSize,
                Handles.CubeHandleCap,
                0f
            );

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(hitbox, "Change Box Size Z");

                data.Size.z =
                    Mathf.Max(
                        0.01f,
                        Vector3.Distance(center, newZPosition) * 2f
                    );

                EditorUtility.SetDirty(hitbox);
            }
        }

        private void DrawBox(
            Vector3 center,
            Quaternion rotation,
            Vector3 size)
        {
            Matrix4x4 oldMatrix = Handles.matrix;

            Handles.matrix = Matrix4x4.TRS(
                center,
                rotation,
                Vector3.one
            );

            Handles.color = Color.cyan;

            Handles.DrawWireCube(
                Vector3.zero,
                size
            );

            Handles.matrix = oldMatrix;
        }

        private void ProcessCapsuleShape(HitboxMeta hitbox)
        {
            if (hitbox.Shape != HitboxShape.Capsule)
                return;

            CapsuleHitboxData data = hitbox.Data.Capsule;

            GetBaseTransforms(hitbox, data,
                out var center,
                out var rotation);


            Handles.color = Color.cyan;

            DrawCapsule(
                center,
                rotation,
                data.Radius,
                data.Height
            );

            EditorGUI.BeginChangeCheck();

            float newRadius = Handles.RadiusHandle(
                rotation,
                center,
                data.Radius);


            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(hitbox, "Change Hitbox Radius");

                data.Radius = Mathf.Max(
                    0.01f,
                    newRadius
                );

                EditorUtility.SetDirty(hitbox);
            }


            Vector3 up = rotation * Vector3.up;

            Vector3 top =
                center + up * (data.Height / 2f);

            EditorGUI.BeginChangeCheck();

            Vector3 newTop = Handles.Slider(
                top,
                up,
                HandleUtility.GetHandleSize(top) * 0.15f,
                Handles.CubeHandleCap,
                0f
            );

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(hitbox, "Change Hitbox Height");

                float newHeight =
                    Vector3.Distance(center, newTop) * 2f;

                data.Height = Mathf.Max(
                    data.Radius * 2f,
                    newHeight
                );

                EditorUtility.SetDirty(hitbox);
            }
        }


        private void DrawCapsule(
            Vector3 center,
            Quaternion rotation,
            float radius,
            float height)
        {
            Vector3 up = rotation * Vector3.up;

            float halfHeight = Mathf.Max(height / 2f, radius);

            Vector3 top = center + up * (halfHeight - radius);
            Vector3 bottom = center - up * (halfHeight - radius);

            Handles.color = Color.cyan;

            Handles.DrawWireDisc(top, up, radius);
            Handles.DrawWireDisc(bottom, up, radius);

            Handles.DrawWireArc(
                top,
                rotation * Vector3.right,
                rotation * Vector3.forward,
                -180f,
                radius
            );

            Handles.DrawWireArc(
                top,
                rotation * Vector3.forward,
                rotation * Vector3.right,
                180f,
                radius
            );

            Handles.DrawWireArc(
                bottom,
                rotation * Vector3.right,
                rotation * Vector3.forward,
                180f,
                radius
            );

            Handles.DrawWireArc(
                bottom,
                rotation * Vector3.forward,
                rotation * Vector3.right,
                -180f,
                radius
            );

            // Четыре боковые линии
            Vector3 right = rotation * Vector3.right;
            Vector3 forward = rotation * Vector3.forward;

            Handles.DrawLine(
                top + right * radius,
                bottom + right * radius
            );

            Handles.DrawLine(
                top - right * radius,
                bottom - right * radius
            );

            Handles.DrawLine(
                top + forward * radius,
                bottom + forward * radius
            );

            Handles.DrawLine(
                top - forward * radius,
                bottom - forward * radius
            );
        }

        private void GetBaseTransforms(HitboxMeta hitbox, HitboxShapeData data, out Vector3 center,
            out Quaternion rotation)
        {
            center = hitbox.transform.TransformPoint(data.Center);
            rotation = hitbox.transform.rotation * data.Rotation;
        }
    }
}
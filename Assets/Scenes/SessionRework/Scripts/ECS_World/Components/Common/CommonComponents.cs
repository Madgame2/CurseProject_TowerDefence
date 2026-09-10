using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Common
{
    public struct IDComponent : IComponent  { public uint NetId; }

    public struct UnityViewComponent : IComponent
    {
        public GameObject GameObject;
        public Transform Transform;
        public Animator Animator;
    }

    public struct LookAtComponent : IComponent
    {
        public Vector3 Position;
    }
}
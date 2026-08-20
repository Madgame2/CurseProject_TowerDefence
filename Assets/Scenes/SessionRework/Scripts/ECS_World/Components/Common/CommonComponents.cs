using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Common
{
    public struct IDComponent : IComponent  { public Guid Id; }

    public struct UnityViewComponent : IComponent
    {
        public GameObject GameObject;
        public Transform Transform;
    }
}
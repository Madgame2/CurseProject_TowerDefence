using Scellecs.Morpeh;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Simulation.Requests
{
    public struct SetPositionRequest:IComponent
    {
        public Vector3 NewPosition;
    }
}
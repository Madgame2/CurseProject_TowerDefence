using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.Player.Enums;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Simulation.Requests
{
    public struct SetPositionRequest:IComponent
    {
        public Vector3 NewPosition;
    }
    
    public struct SetVelocityRequest: IComponent
    {
        public Vector3 NewVelocity;
    }

    public struct SetMovementStateRequest : IComponent
    {
        public MovementState NewMovementState;
    }
}
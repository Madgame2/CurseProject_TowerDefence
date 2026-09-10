using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Addition;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Movement
{
    public struct InputComponent : IComponent
    {
        public Vector3 MoveDirection;
    }
    
    public struct MoveInputHistoryComponent : IComponent
    {
        public MoveInputCommand[] Buffer;
        public int CurrentIndex;
    }
    
    public struct JumpRequestComponent : IComponent{}
}
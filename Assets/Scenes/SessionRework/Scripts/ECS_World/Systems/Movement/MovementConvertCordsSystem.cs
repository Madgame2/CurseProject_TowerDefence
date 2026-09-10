using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Movement
{
    public class MovementConvertCordsSystem: IFixedSystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<InputComponent> _inputStash;
        private Stash<LookAtComponent> _lookAtStash;
        private Stash<PositionComponent> _positionStash;
        
        public MovementConvertCordsSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<IDComponent>()
                .With<LookAtComponent>()
                .With<PlayerComponent>()
                .With<PositionComponent>()
                .With<InputComponent>()
                .Build();
            
            _lookAtStash = World.GetStash<LookAtComponent>();
            _inputStash = World.GetStash<InputComponent>();
            _positionStash = World.GetStash<PositionComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var input = ref _inputStash.Get(entity);
                ref var lookAtComponent = ref _lookAtStash.Get(entity);
                ref var positionComponent = ref _positionStash.Get(entity);
                
                var forwardDirection = lookAtComponent.Position - positionComponent.Position;
                forwardDirection.y = 0;
                
                var rotation = Quaternion.LookRotation(forwardDirection);
                
                input.MoveDirection = rotation * input.MoveDirection;
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}
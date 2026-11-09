using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Movement;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Input
{
    public class JumpInputSystem : IFixedSystem
    {
        public World World { get; set; }
        
        private Filter _filter;

        private Stash<JumpRequestComponent> _jumpRequestStash;
        
        private readonly BaseInputActions _inputs;
        
        public JumpInputSystem(World world,  BaseInputActions inputs)
        {
            World = world;
            _inputs = inputs;
        }
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<IDComponent>()
                .With<MyPlayerComponent>()
                .Build();
            
            _jumpRequestStash = World.GetStash<JumpRequestComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            var jumpRequested = _inputs.SessionPlayer.Jump.IsPressed();
            foreach (var entity in _filter)
            {
                if (!jumpRequested)
                    continue;
             
                _jumpRequestStash.Set(entity);
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}
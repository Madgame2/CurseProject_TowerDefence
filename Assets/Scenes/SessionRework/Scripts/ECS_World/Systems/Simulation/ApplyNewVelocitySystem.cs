using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Simulation.Requests;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems
{
    public class ApplyNewVelocitySystem: ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        
        private Stash<VelocityComponent> _velocityStash;
        private Stash<SetVelocityRequest> _setVelocityStash;
        
        public ApplyNewVelocitySystem(World world)
        {
            World = world;
        }
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<VelocityComponent>()
                .With<SetVelocityRequest>()
                .Build();
            
            _velocityStash = World.GetStash<VelocityComponent>();
            _setVelocityStash = World.GetStash<SetVelocityRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var velocityComponent = ref _velocityStash.Get(entity);
                ref var setVelocityComponent = ref _setVelocityStash.Get(entity);

                velocityComponent.WorldVelocity = setVelocityComponent.NewVelocity;
                
                _setVelocityStash.Remove(entity);
            }
        }
        public void Dispose()
        {
            
        }
    }
}
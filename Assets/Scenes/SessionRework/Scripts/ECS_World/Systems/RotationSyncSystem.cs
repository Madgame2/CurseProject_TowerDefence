using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems
{
    public class RotationSyncSystem: ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<RotationComponent> _rotationStash;
        private Stash<UnityViewComponent> _unityViewStash;
        
        public RotationSyncSystem(World world)
        {
            World = world;
        }
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<RotationComponent>()
                .With<UnityViewComponent>()
                .Build();
            
            _rotationStash = World.GetStash<RotationComponent>();
            _unityViewStash = World.GetStash<UnityViewComponent>();   
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotationComponent = ref _rotationStash.Get(entity);
                ref var view = ref _unityViewStash.Get(entity);
            
                if (view.Transform != null)
                {
                    view.Transform.rotation = rotationComponent.Rotation;
                }
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}
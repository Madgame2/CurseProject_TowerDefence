using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Camera
{
    public class CameraSyncSystem: ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        
        private Stash<CameraViewComponent> _cameraViewStash;
        private Stash<PositionComponent> _positionStash;
        private Stash<RotationComponent> _rotationStash;
        
        
        public CameraSyncSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<CameraViewComponent>()
                .With<PositionComponent>()
                .With<RotationComponent>()
                .Build();
            
            _cameraViewStash = World.GetStash<CameraViewComponent>();
            _positionStash = World.GetStash<PositionComponent>();
            _rotationStash = World.GetStash<RotationComponent>();
        }


        public void OnUpdate(float deltaTime)
        {
            foreach (var cameraEntity in _filter)
            {
                ref var positionComponent = ref _positionStash.Get(cameraEntity);
                ref var rotationComponent = ref _rotationStash.Get(cameraEntity);
                ref var cameraViewComponent = ref _cameraViewStash.Get(cameraEntity);
                
                var cameraTransform = cameraViewComponent.CameraTransform;
                if(cameraTransform == null)
                    continue;
                
                cameraTransform.position = positionComponent.Position;
                cameraTransform.rotation = rotationComponent.Rotation;
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}
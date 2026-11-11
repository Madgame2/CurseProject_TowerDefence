using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Input
{
    public class ApplyCameraStateSystem: ISystem
    {
        public World World { get; set; }
        
        private Filter _cameraFilter;
        private Stash<CameraStateComponent> _cameraStateStash;
        private Stash<SetCameraStateRequest> _setCameraStateStash;

        public ApplyCameraStateSystem(World world)
        {
            World = world;
        }
        
        public void OnAwake()
        {
            _cameraFilter = World.Filter
                .With<CameraStateComponent>()
                .With<SetCameraStateRequest>()
                .Build();
            
            _cameraStateStash = World.GetStash<CameraStateComponent>();
            _setCameraStateStash = World.GetStash<SetCameraStateRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _cameraFilter)
            {
                ref var cameraStateComponent = ref _cameraStateStash.Get(entity);
                ref var setCameraStateComponent = ref _setCameraStateStash.Get(entity);

                cameraStateComponent.CameraState = setCameraStateComponent.NewCameraState;
                
                _setCameraStateStash.Remove(entity);
            }
        }
        
        public void Dispose()
        {
        }
    }
}
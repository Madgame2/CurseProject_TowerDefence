using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Systems
{
    public class LookAtCameraTargetSystem: ISystem
    {
        public World World { get; set; }

        private Filter _cameraFilter;
        private Filter _playerFilter;

        private Stash<CameraViewComponent> _cameraViewStash;
        private Stash<LookAtComponent>  _lookAtStash;
        
        private const float MaxAimDistance = 100f;
        private readonly LayerMask _aimLayerMask = ~LayerMask.GetMask("Player", "Ignore Raycast");

        
        public LookAtCameraTargetSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _cameraFilter = World.Filter
                .With<CameraViewComponent>()
                .Build();
            
            
            _playerFilter = World.Filter
                .With<PlayerComponent>()
                .With<LookAtComponent>()
                .With<CameraTargetComponent>()
                .Build();
            
            _cameraViewStash = World.GetStash<CameraViewComponent>();
            _lookAtStash =  World.GetStash<LookAtComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            if(_cameraFilter.IsEmpty())
                return;
            
            var cameraEntity = _cameraFilter.GetEntity(0);

            ref var cameraView = ref _cameraViewStash.Get(cameraEntity);


            var cameraTransform = cameraView.CameraTransform;
            
            Vector3 targetAimPosition;
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, MaxAimDistance, _aimLayerMask))
            {
                targetAimPosition = hit.point;
            }
            else
            {
                targetAimPosition = ray.origin + ray.direction * MaxAimDistance;
            }
            
            
            foreach (var playerEntity in _playerFilter)
            {
                ref var lookAtComponent = ref _lookAtStash.Get(playerEntity);
                
                lookAtComponent.Position = targetAimPosition;
            }
        }
        
        public void Dispose()
        {
        }
    }
}
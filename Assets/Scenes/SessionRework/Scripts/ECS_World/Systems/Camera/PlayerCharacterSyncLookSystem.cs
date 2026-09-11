using System.Diagnostics.SymbolStore;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Camera
{
    public class PlayerCharacterSyncLookSystem: ISystem
    {
        public World World { get; set; }
        
        private Filter _playerFilter;
        private Filter _cameraFilter;

        private Stash<PlayerComponent> _playerStash;
        private Stash<CameraViewComponent> _cameraStash;
        
        private const float MaxAimDistance = 100f;
        
        private readonly LayerMask _aimLayerMask = ~LayerMask.GetMask("Player", "Ignore Raycast");
        
        public void OnAwake()
        {
            _cameraFilter = World.Filter
                .With<CameraViewComponent>()
                .With<RotationStateComponent>()
                .Build();
            
            _playerFilter = World.Filter
                .With<PlayerComponent>()
                .With<CameraTargetComponent>().Build();
            
            
            _playerStash = World.GetStash<PlayerComponent>();
            _cameraStash =  World.GetStash<CameraViewComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_cameraFilter.IsEmpty() || _playerFilter.IsEmpty()) return;
            
            var cameraEntity = _cameraFilter.GetEntity(0);
            ref var cameraComponent = ref _cameraStash.Get(cameraEntity);
            
            var cameraTransform = cameraComponent.CameraTransform;
            if (cameraTransform == null) return;
            
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
                ref var playerComponent = ref _playerStash.Get(playerEntity);
                var playerView = playerComponent.PlayerView;

                if (playerView == null || playerView.PlayerLooTarget == null) continue;
                
                playerView.PlayerLooTarget.position = Vector3.Lerp(
                    playerView.PlayerLooTarget.position, 
                    targetAimPosition, 
                    deltaTime * 20f
                );
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}
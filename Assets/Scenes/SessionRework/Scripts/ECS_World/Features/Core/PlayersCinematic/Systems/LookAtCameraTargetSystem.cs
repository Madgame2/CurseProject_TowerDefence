using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.GameWorld.View;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Systems
{
    public class LookAtCameraTargetSystem: ISystem
    {
        public World World { get; set; }
        
        private Filter _playerFilter;
        private Stash<LookAtComponent>  _lookAtStash;
        private CameraView _cameraView;
        
        public LookAtCameraTargetSystem(World world,CameraView cameraView)
        {
            World = world;
            _cameraView = cameraView;
        }

        public void OnAwake()
        {
            _playerFilter = World.Filter
                .With<MyPlayerComponent>()
                .With<LookAtComponent>()
                .Build();
            
            _lookAtStash =  World.GetStash<LookAtComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            var lookAt = _cameraView.LookAtPoint;
            
            
            foreach (var playerEntity in _playerFilter)
            {
                
                ref var lookAtComponent = ref _lookAtStash.Get(playerEntity);
                
                lookAtComponent.Position = lookAt;
            }
        }
        
        public void Dispose()
        {
        }
    }
}
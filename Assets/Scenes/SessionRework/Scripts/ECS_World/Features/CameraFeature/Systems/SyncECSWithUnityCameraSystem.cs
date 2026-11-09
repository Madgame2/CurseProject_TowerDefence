using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.Cameras.Controller.Interfaces;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;
using Scenes.SessionRework.Scripts.ECS_World.Features.Services.TypesMapper.Interfaces;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Input
{
    public class SyncECSWithUnityCameraSystem:ISystem
    {
        public World World { get; set; }
        private Filter _filter;
        private Stash<CameraStateComponent> _cameraStateStash;
        
        private readonly ICameraController _cameraController;
        private readonly ICameraStateMapper _cameraStateMapper;

        public SyncECSWithUnityCameraSystem(World world,  ICameraController cameraController, ICameraStateMapper  cameraStateMapper)
        {
            World = world;
            _cameraController = cameraController;
            _cameraStateMapper = cameraStateMapper;
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<CameraStateComponent>().Build();
            
            _cameraStateStash = World.GetStash<CameraStateComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            var cameraEntity =  _filter.First();

            ref var cameraStateEntity = ref _cameraStateStash.Get(cameraEntity);
            
            var cameraStateType = _cameraStateMapper.Map(cameraStateEntity.CameraState);

            if (cameraStateType == null)
            {
                Debug.LogError($"CameraStateMapper.Map(cameraStateEntity.CameraState) is null!");
                return;
            }
            
            _cameraController.TryMoveToState(cameraStateType);
        }
        
        public void Dispose()
        {
            
        }
    }
}
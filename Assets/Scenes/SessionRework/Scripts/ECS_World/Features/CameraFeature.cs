using Scenes.SessionRework.Scripts.ECS_World.Systems.Input;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Camera;
using Scenes.SessionRework.Scripts.Player.View;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class CameraFeature
    {
        private static Filter _playerFilter;
        

        public static void AddFeature(
            World world,
            SystemsGroup systemsGroup,
            DiContainer container)
        {
            CreateFilters(world);

            InitializeCameraEntity(world, container);
            AddCharacterViewToPlayer(world);

            AddCameraSystems(systemsGroup, container);
        }

        private static void CreateFilters(World world)
        {
            _playerFilter = world.Filter
                .With<UnityViewComponent>()
                .With<PlayerComponent>()
                .With<MyPlayerComponent>()
                .Build();
        }

        private static void InitializeCameraEntity(
            World world,
            DiContainer container)
        {
            var cameraEntity = world.CreateEntity();

            world.GetStash<LookInputComponent>()
                .Set(cameraEntity);

            world.GetStash<RotationStateComponent>()
                .Set(cameraEntity);

            var cameraView = container.TryResolve<CameraView>();

            if (cameraView == null)
            {
                const string errorMessage = "CameraView was not found.";

                Debug.LogError(errorMessage);
                throw new System.Exception(errorMessage);
            }

            world.GetStash<CameraViewComponent>()
                .Set(cameraEntity, new CameraViewComponent
                {
                    CameraTransform = cameraView.transform
                });
            
            world.GetStash<PositionComponent>().Set(cameraEntity);
            world.GetStash<RotationComponent>().Set(cameraEntity);
        }

        private static void AddCharacterViewToPlayer(World world)
        {
            var characterViewStash =
                world.GetStash<CharacterViewComponent>();

            foreach (var playerEntity in _playerFilter)
            {
                ref readonly var unityView =
                    ref playerEntity.GetComponent<UnityViewComponent>();

                if (unityView.GameObject == null)
                    continue;

                if(!unityView.GameObject.TryGetComponent<PlayerView>(out var playerView))
                    continue;
                
                characterViewStash.Set(playerEntity, new CharacterViewComponent
                {
                    CharacterRoot = unityView.Transform,
                    SpineBone = playerView.SpineBone,
                });
            }
        }

        private static void AddCameraSystems(
            SystemsGroup systemsGroup,
            DiContainer container)
        {
            
            systemsGroup.AddSystem(
                container.Instantiate<SetTargetSystem>());
            
            systemsGroup.AddSystem(
                container.Instantiate<CameraInputSystem>());

            systemsGroup.AddSystem(
                container.Instantiate<CameraLookSystem>());

            systemsGroup.AddSystem(
                container.Instantiate<ChangeCameraTransformSystem>());

            systemsGroup.AddSystem(
                container.Instantiate<CameraSyncSystem>());

            //systemsGroup.AddSystem(
            //   container.Instantiate<PlayerCharacterSyncLookSystem>());
        }
    }
}

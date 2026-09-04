using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Input;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class CameraFeature
    {
        private static Filter _playerFilter;
        private static Filter _cameraFilter;

        public static void AddFeature(World world, SystemsGroup group, DiContainer container)
        {
            _playerFilter = world.Filter
                .With<UnityViewComponent>()
                .With<PlayerComponent>()
                .With<MyPlayerComponent>()
                .Build();

            _cameraFilter = world.Filter.With<LookInputComponent>()
                .With<RotationStateComponent>()
                .With<CameraRestrictionsComponent>()
                .Build();

            InitCameraEntity(world, container);
            AddComponentsToPlayer(world);
            SetCameraTarget(world);

            group.AddSystem(container.Instantiate<CameraInputSystem>());
            group.AddSystem(container.Instantiate<PlayerLookSystem>());
            group.AddSystem(container.Instantiate<CameraSyncSystem>());
        }

        private static void InitCameraEntity(World world, DiContainer container)
        {
            var cameraEntity = world.CreateEntity();

            var lookUnputStash = world.GetStash<LookInputComponent>();
            lookUnputStash.Set(cameraEntity);

            var rotationStateStash = world.GetStash<RotationStateComponent>();
            rotationStateStash.Set(cameraEntity);

            var cameraRestrictionsStash = world.GetStash<CameraRestrictionsComponent>();
            cameraRestrictionsStash.Set(cameraEntity, new CameraRestrictionsComponent
            {
                Sensitivity = 1,
                MaxPitch = 60,
                MinPitch = -10
            });

            var cameraView = container.TryResolve<CameraView>();
            if (cameraView == null)
            {
                Debug.LogError("Not found cameraView");
                throw new System.Exception("Not found cameraView");
            }

            var cameraViewStash = world.GetStash<CameraViewComponent>();
            cameraViewStash.Set(cameraEntity, new CameraViewComponent
            {
                CameraTransform = cameraView.transform
            });
        }

        private static void SetCameraTarget(World world)
        {
            var characterViewStash = world.GetStash<CharacterViewComponent>();
            foreach (var playerEntity in _playerFilter)
            {
                ref var playerView = ref characterViewStash.Get(playerEntity);

                var cameraTargetStash = world.GetStash<CameraTargetComponent>();
                cameraTargetStash.Set(playerEntity, new CameraTargetComponent
                {
                    Target = playerView.CharacterRoot,
                    TargetDistance = 10
                });
                break;
            }
        }


        private static void AddComponentsToPlayer(World world)
        {
            foreach (var entity in _playerFilter)
            {
                ref readonly var unityView = ref entity.GetComponent<UnityViewComponent>();

                if (unityView.GameObject == null) continue;

                var characterViewStash = world.GetStash<CharacterViewComponent>();
                characterViewStash.Set(entity, new CharacterViewComponent
                {
                    CharacterRoot = unityView.Transform
                });
            }
        }
    }
}
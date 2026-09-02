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
        public static void AddFeature(World world, SystemsGroup group, DiContainer container)
        {
            InitCameraEntity(world,container);
            AddComponentsToPlayer(world);

            group.AddSystem(container.Instantiate<CameraInputSystem>());
            group.AddSystem(container.Instantiate<PlayerLookSystem>());
            group.AddSystem(container.Instantiate<CameraSyncSystem>());
        }

        private static void AddComponentsToPlayer(World world)
        {
            var playerFilter = world.Filter
                .With<UnityViewComponent>()
                .With<PlayerComponent>()
                .With<MyPlayerComponent>()
                .Build();

            foreach (var entity in playerFilter)
            {
                ref readonly var unityView = ref entity.GetComponent<UnityViewComponent>();
                
                if (unityView.GameObject == null) continue;
                
                var characterViewStash = world.GetStash<CharacterViewComponent>();
                characterViewStash.Set(entity, new CharacterViewComponent
                {
                    CharacterRoot = unityView.Transform
                });
                
                var cameraTargetStash = world.GetStash<CameraTargetComponent>();
                cameraTargetStash.Set(entity,new CameraTargetComponent
                {
                    TargetDistance = 10
                });
            }
        }

        private static void InitCameraEntity(World world,DiContainer container)
        {
            var cameraEntity = world.CreateEntity();

            var lookUnputStash = world.GetStash<LookInputComponent>();
            lookUnputStash.Set(cameraEntity);
            
            var rotationStateStash = world.GetStash<RotationStateComponent>();
            rotationStateStash.Set(cameraEntity, new RotationStateComponent
            {
                Sensitivity = 1,
                MaxPitch = 60,
                MinPitch = -10
            });
            
            var cameraView = container.TryResolve<CameraView>();
            if (cameraView == null)
            {
                Debug.LogError("Not found cameraView");
                return;
            }
            
            var cameraViewStash = world.GetStash<CameraViewComponent>();
            cameraViewStash.Set(cameraEntity, new CameraViewComponent
            {
                CameraTransform = cameraView.transform
            });
        }
    }
}
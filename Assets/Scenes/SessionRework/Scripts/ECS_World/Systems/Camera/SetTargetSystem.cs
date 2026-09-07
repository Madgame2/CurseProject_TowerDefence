using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.Player.View;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Camera
{
    public class SetTargetSystem: ISystem
    {
        public World World { get; set; }

        private Filter _cameraFilter;
        private Filter _playerFilter;

        public SetTargetSystem(World world)
        {
            World = world;
        }

        public void OnAwake()
        {
            _cameraFilter = World.Filter
                .With<LookInputComponent>()
                .With<RotationStateComponent>()
                .With<CameraViewComponent>()
                .Build();
         
            _playerFilter = World.Filter
                .With<UnityViewComponent>()
                .With<PlayerComponent>()
                .With<MyPlayerComponent>()
                .Build();
            
            ConfigureCameraTarget(World);
        }
        
        public void OnUpdate(float deltaTime)
        {
            
        }
        
        public void Dispose()
        {
            
        }
        
        private void ConfigureCameraTarget(World world)
        {
            foreach (var playerEntity in _playerFilter)
            {
                if (!TryGetPlayerView(playerEntity, out var playerView))
                    continue;

                ConfigurePlayerCameraTarget(world, playerEntity, playerView);
                ConfigureCameraRestrictions(world, playerView);

                return;
            }
        }
        
        private static bool TryGetPlayerView(
            Scellecs.Morpeh.Entity playerEntity,
            out PlayerView playerView)
        {
            ref readonly var unityView =
                ref playerEntity.GetComponent<UnityViewComponent>();

            if (unityView.GameObject == null)
            {
                playerView = null;
                return false;
            }

            return unityView.GameObject.TryGetComponent(out playerView);
        }
        
        private void ConfigurePlayerCameraTarget(
            World world,
            Scellecs.Morpeh.Entity playerEntity,
            PlayerView playerView)
        {
            ref readonly var characterView =
                ref playerEntity.GetComponent<CharacterViewComponent>();

            Vector3 targetPosition =
                playerView.CameraTargetLook.position;

            Vector3 characterPosition =
                characterView.CharacterRoot.position;

            world.GetStash<CameraTargetComponent>()
                .Set(playerEntity, new CameraTargetComponent
                {
                    PivotOffset = targetPosition - characterPosition,
                    Target = characterView.CharacterRoot,
                    TargetDistances =
                        playerView.CameraTargetLook.position -
                        playerView.CameraStartPosition.position
                });
        }
        
        private void ConfigureCameraRestrictions(
            World world,
            PlayerView playerView)
        {
            foreach (var cameraEntity in _cameraFilter)
            {
                world.GetStash<CameraRestrictionsComponent>()
                    .Set(cameraEntity, new CameraRestrictionsComponent
                    {
                        Sensitivity = 1f,
                        MaxPitch = playerView.CameraSettingsDefinition.MaxPitch,
                        MinPitch = playerView.CameraSettingsDefinition.MinPitch
                    });

                return;
            }
        }
    }
}
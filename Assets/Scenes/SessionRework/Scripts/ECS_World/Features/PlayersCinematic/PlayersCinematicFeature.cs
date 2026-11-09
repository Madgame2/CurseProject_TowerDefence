using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Animations;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.PlayersCinematic.Systems;
using Scenes.SessionRework.Scripts.ECS_World.Features.PlayersCinematic.Systems._TEMP;
using Scenes.SessionRework.Scripts.ECS_World.Features.Systems;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Animation;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Camera;
using Scenes.SessionRework.Scripts.Player.View;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class PlayersCinematicFeature
    {
        public static void AddFeature(World world, SystemsGroup updateGroup, DiContainer container)
        {
            updateGroup.AddSystem(container.Instantiate<AddComponentsToPlayer>());
            
            updateGroup.AddSystem(container.Instantiate<PlayerMoveAnimationSystem>());
            updateGroup.AddSystem(container.Instantiate<LookAtCameraTargetSystem>());
            updateGroup.AddSystem(container.Instantiate<SyncUnityCharacterLookSystem>());
            updateGroup.AddSystem(container.Instantiate<RotateWhenMovingSystem>()); 
            updateGroup.AddSystem(container.Instantiate<GazeRotationAngleSystem>());
            updateGroup.AddSystem(container.Instantiate<PlayRotationInPlaceAnimations>());
            updateGroup.AddSystem(container.Instantiate<PlayerJumpAnimationSystem>());
        }
    }
}
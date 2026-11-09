using Zenject;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.GlobalGamePlayState;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Input;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Installers
{
    public static class EcsWorldInstaller
    {
        public static void Install(
            Scellecs.Morpeh.World world,
            DiContainer container)
        {
            var updateGroup = world.CreateSystemsGroup();
            var fixedUpdateGroup = world.CreateSystemsGroup();
            var lateUpdateGroup = world.CreateSystemsGroup();
            
            
            GlobalGamePlayStateFeature.AddFeature(world, updateGroup, container);
            InputFeature.AddFeature(world, updateGroup, container);
            PlayersCinematicFeature.AddFeature(world, updateGroup, container);
            ChunkLoadingFeature.AddFeature(world, updateGroup, container);
            SimulationFeature.AddFeature(world, fixedUpdateGroup, container);
            NetworkFeature.AddFeature(world, fixedUpdateGroup,container);
            CameraFeature.AddFeature(world, lateUpdateGroup, container);
            CharactersFeature.AddFeature(world, updateGroup, container);
            SyncEcsWithUnityFeature.AddFeature(world, updateGroup, container);

            AlternativeActionFeature.AddFeature(world, updateGroup, container);
            PlayerMovementFeature.AddFeature(world, updateGroup,container);
            
            world.AddSystemsGroup(0, fixedUpdateGroup);
            world.AddSystemsGroup(1,updateGroup);
            world.AddSystemsGroup(2, lateUpdateGroup);
        }
    }
}
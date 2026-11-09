using System.ComponentModel;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Input;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.Cameras.Controller.Implementation;
using Scenes.SessionRework.Scripts.Cameras.Interfaces;
using Scenes.SessionRework.Scripts.Cameras.Services;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.Interfaces;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.AimingState.Factory;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Factory;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState.Extentions;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState.Factory;
using Scenes.SessionRework.Scripts.ECS_World.Components.Camera;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Components;
using Scenes.SessionRework.Scripts.ECS_World.Features.Services.TypesMapper.Implementation;
using Scenes.SessionRework.Scripts.ECS_World.Features.Services.TypesMapper.Interfaces;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Camera;
using Scenes.SessionRework.Scripts.GameWorld.View;
using Scenes.SessionRework.Scripts.Player.View;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class CameraFeature
    {
        
        public static void AddFeature(
            World world,
            SystemsGroup systemsGroup,
            DiContainer container)
        {
            InitializeCameraEntity(world, container);
            RegisterServices(world,container);
            AddCameraSystems(systemsGroup, container);
        }

        private static void RegisterServices(World world, DiContainer container)
        {
            Debug.Log($"Register CameraController. Container ID: {container.GetHashCode()}");

            container.Bind<CameraInputService>()
                .AsSingle();

            container.Bind<ICameraInput>()
                .To<CameraInputService>()
                .FromResolve();
            
            container.Bind<ICameraStateMapper>().To<CameraStateMapper>().AsSingle();
            
            container.RegisterFollowCameraState();
            container.RegisterAimingCameraState();
            
            container.Bind<IStateFactory>().To<StateFactory>().AsSingle();
            container.Bind<ICameraStateMachine>().To<CameraStateMachine>().AsSingle();
            container.BindInterfacesAndSelfTo<CameraController>().AsSingle().NonLazy();
        }

        private static void InitializeCameraEntity(
            World world,
            DiContainer container)
        {
            var cameraEntity = world.CreateEntity();

            world.GetStash<LookInputComponent>().Set(cameraEntity);
            world.GetStash<CameraStateComponent>().Set(cameraEntity);
        }
        

        private static void AddCameraSystems(
            SystemsGroup systemsGroup,
            DiContainer container)
        {
            systemsGroup.AddSystem(container.Instantiate<CameraInputSystem>());
        }
    }
}

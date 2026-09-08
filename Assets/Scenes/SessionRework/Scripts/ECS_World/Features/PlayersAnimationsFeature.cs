using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Animations;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Animation;
using Scenes.SessionRework.Scripts.ECS_World.Systems.Camera;
using Scenes.SessionRework.Scripts.Player.View;
using UnityEngine;
using Zenject;

namespace Scenes.SessionRework.Scripts.ECS_World.Features
{
    public static class PlayersAnimationsFeature
    {
        public static void AddFeature(World world, SystemsGroup updateGroup, DiContainer container)
        {
            AddComponentsToPlayers(world);
            
            updateGroup.AddSystem(container.Instantiate<PlayerAnimationSystem>());
            updateGroup.AddSystem(container.Instantiate<PlayerTurnInPlaceSystem>());
            updateGroup.AddSystem(container.Instantiate<SpineLookAtTargetInverseKinematicsSystem>());
        }

        private static void AddComponentsToPlayers(World world)
        {
            Filter filter = world.Filter
                .With<PlayerComponent>()
                .With<VelocityComponent>()
                .With<UnityViewComponent>()
                .Build();

            Stash<UnityViewComponent> unityViewStash = world.GetStash<UnityViewComponent>();
            Stash<AnimatorReferenceComponent>  animatorReferenceStash = world.GetStash<AnimatorReferenceComponent>();
            Stash<HumanoidBonesComponent> humanoidBonesStash = world.GetStash<HumanoidBonesComponent>();
            Stash<TurnInPlaceComponent>  turnInPlaceStash = world.GetStash<TurnInPlaceComponent>();
            foreach (var playerEntity in filter)
            {
                ref var unityViewComponent = ref unityViewStash.Get(playerEntity);

                animatorReferenceStash.Set(playerEntity, new AnimatorReferenceComponent
                {
                    Animator = unityViewComponent.Animator,
                });
                
                if(!unityViewComponent.GameObject.TryGetComponent<PlayerView>(out var playerView))
                    continue;
                
                humanoidBonesStash.Set(playerEntity, new HumanoidBonesComponent
                {
                    SpineBone = playerView.SpineBone
                });
                
                turnInPlaceStash.Set(playerEntity, new TurnInPlaceComponent
                {
                    TurnThreshold = 80f,        // Начинать переступание при отклонении камеры больше чем на 80°
                    WeightBlendSpeed = 6f,      // Скорость сглаживания веса IK (чем выше, тем быстрее гаснет IK)
                    TotalIKWeight = 1f,         // Изначально IK полностью включен
                    TargetIKWeight = 1f,        // Целевое значение IK (1 = включен)
                    IsTurning = false           // На старте персонаж не поворачивается
                });
            }
        }
    }
}
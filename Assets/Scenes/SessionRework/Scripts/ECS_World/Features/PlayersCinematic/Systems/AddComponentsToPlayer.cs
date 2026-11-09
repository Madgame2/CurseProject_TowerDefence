using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Animations;
using Scenes.SessionRework.Scripts.ECS_World.Components.Common;
using Scenes.SessionRework.Scripts.ECS_World.Components.Geometry;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.Player.View;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.PlayersCinematic.Systems
{
    public class AddComponentsToPlayer: ISystem
    {
        public World World { get; set; }
        
        private Filter _filter;

        private Stash<UnityViewComponent> _unityViewStash;
        private Stash<AnimatorReferenceComponent> _animatorReferenceStash;
        private Stash<HumanoidBonesComponent> _humanoidBonesStash;
        private Stash<TurnInPlaceComponent> _turnInPlaceStash;
        private Stash<LookAtComponent> _lookAtStash;
        
        public AddComponentsToPlayer(World world)
        {
            World = world;
        }
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<PlayerComponent>()
                .With<UnityViewComponent>()
                .Without<AnimatorReferenceComponent>()
                .Build();
                
            _unityViewStash  = World.GetStash<UnityViewComponent>();
            _animatorReferenceStash = World.GetStash<AnimatorReferenceComponent>();
            _humanoidBonesStash = World.GetStash<HumanoidBonesComponent>();
            _turnInPlaceStash = World.GetStash<TurnInPlaceComponent>();
            _lookAtStash = World.GetStash<LookAtComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in _filter)
            {
                ref var unityViewComponent = ref _unityViewStash.Get(playerEntity);

                _animatorReferenceStash.Set(playerEntity, new AnimatorReferenceComponent
                {
                    Animator = unityViewComponent.Animator,
                });
                
                if(!unityViewComponent.GameObject.TryGetComponent<PlayerView>(out var playerView))
                    continue;
                
                _humanoidBonesStash.Set(playerEntity, new HumanoidBonesComponent
                {
                    SpineBone = playerView.SpineBone
                });
                
                _lookAtStash.Set(playerEntity);
            }
        }
        
        public void Dispose()
        {
        }
    }
}
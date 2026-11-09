using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.GlobalGamePlayState.Components;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.GlobalGamePlayState.Meta;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.GlobalGamePlayState.Systems
{
    public class SetGamePlayStateToDefaultSystem:ISystem
    {
        public World World { get; set; }
        private Filter _filter;

        private Stash<GamePlayStateComponent> _gameplayStateStash;
        
        public SetGamePlayStateToDefaultSystem(World world)
        {
            World = world;
        }
        
        public void OnAwake()
        {
            _filter = World.Filter.With<GamePlayStateComponent>()
                .Without<LockComponent>()
                .Build();
            
            _gameplayStateStash = World.GetStash<GamePlayStateComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var globalGameplayEntity in _filter)
            {
                ref var globalGameplayStateComponent = ref _gameplayStateStash.Get(globalGameplayEntity);

                globalGameplayStateComponent.GameplayState = GameplayState.Default;
            }
        }
        
        public void Dispose()
        {
        }
    }
}
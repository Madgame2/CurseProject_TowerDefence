using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.PrimaryAction.Components;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.PrimaryAction.Systems
{
    public class AddToPlayerComponentsSystem: IFixedSystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<PrimaryActionComponent> _primaryActionStash;
        
        public AddToPlayerComponentsSystem(World world)
        {
            World = world;
        }
        
        public void OnAwake()
        {
            _filter = World.Filter
                .With<MyPlayerComponent>()
                .Without<PrimaryActionComponent>()
                .Build();
            
            _primaryActionStash = World.GetStash<PrimaryActionComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var playerEntity in _filter)
            {
                _primaryActionStash.Set(playerEntity);
            }
        }
        
        public void Dispose()
        {
           
        }

    }
}
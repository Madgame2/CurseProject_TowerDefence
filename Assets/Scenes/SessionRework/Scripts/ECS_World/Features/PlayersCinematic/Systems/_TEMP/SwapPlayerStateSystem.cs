using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.PlayersCinematic.Systems._TEMP
{
    public class SwapPlayerStateSystem: ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<PlayerComponent> _playerStash;
        
        private readonly BaseInputActions _inputs;
        
        
        public SwapPlayerStateSystem(World world, BaseInputActions inputs)
        {
            World = world;
            _inputs = inputs;
        }
        
        public void OnAwake()
        {
            _filter = World.Filter.With<PlayerComponent>().With<MyPlayerComponent>().Build();
            
            _playerStash = World.GetStash<PlayerComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            var rightClick = _inputs.SessionPlayer.RightClick;
            var rightClickPressed = rightClick.IsPressed();

            var playerEntity = _filter.First();
            ref var playerComponent = ref _playerStash.Get(playerEntity);
            var playerView = playerComponent.PlayerView;

            playerView.Animator.SetBool("IsAiming", rightClickPressed);

            if (rightClick.WasPressedThisFrame())
            {
                playerView.Animator.SetTrigger("StartAiming");
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}
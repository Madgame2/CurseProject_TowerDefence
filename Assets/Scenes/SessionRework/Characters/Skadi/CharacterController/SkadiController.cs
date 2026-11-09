using Scenes.SessionRework.Characters.Skadi.CharacterController.Modules;
using Scenes.SessionRework.Scripts.Cameras.Controller.Interfaces;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Meta;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Meta;
using Scenes.SessionRework.Scripts.Player.View;

namespace Scenes.SessionRework.Characters.Skadi.CharacterController
{
    public class SkadiController: CharacterControllerBase, IAlternativeActiveable
    {
        public IAlternativeActionModule AlternativeActionModule { get; }

        public SkadiController(ICameraController cameraController, PlayerView playerView)
        {
            AlternativeActionModule = new AlternativeActionModule(cameraController, playerView);
        }
    }
}
using Scenes.SessionRework.Characters.Base;
using Scenes.SessionRework.Characters.Base.Models;
using Scenes.SessionRework.Characters.Skadi.Abilities;
using Scenes.SessionRework.Characters.Skadi.Abilities.Aiming;
using Scenes.SessionRework.Characters.Skadi.CharacterController.Modules;
using Scenes.SessionRework.Scripts.Cameras.Controller.Interfaces;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Meta;
using Scenes.SessionRework.Scripts.Player.View;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Characters.Skadi.CharacterController
{
    public class SkadiController : CharacterControllerBase
    {

        public SkadiController(
            ICameraController cameraController,
            PlayerView playerView,
            bool isLocallyControlled)
        {
            RegisterInput(
                CharacterInput.AlternativeAction,
                (ushort)SkadiAbility.Aiming);
            
            RegisterAbility(
                new SkadiAimingAbility(
                    cameraController,
                    playerView,
                    isLocallyControlled));
        }
    }
}
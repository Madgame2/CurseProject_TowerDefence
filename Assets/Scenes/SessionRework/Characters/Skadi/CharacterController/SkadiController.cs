using Scenes.SessionRework.Characters.Skadi.Abilities;
using Scenes.SessionRework.Characters.Skadi.CharacterController.Modules;
using Scenes.SessionRework.Scripts.Cameras.Controller.Interfaces;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Meta;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Meta;
using Scenes.SessionRework.Scripts.Player.View;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Characters.Skadi.CharacterController
{
    public class SkadiController : CharacterControllerBase, IAlternativeActiveable
    {
        public IAlternativeActionModule AlternativeActionModule { get; }

        public SkadiController(ICameraController cameraController, PlayerView playerView)
        {
            AlternativeActionModule = new AlternativeActionModule(cameraController, playerView);
        }

        public override void ProcessAbylity(ushort actionId, PlayerGameplayEventType eventType)
        {
            switch (actionId)
            {
                case (ushort)SkadiAbility.PrimaryAttack:

                    break;
                case (ushort)SkadiAbility.Aiming:
                    processAiming(eventType);
                    break;
            }
        }

        private void processAiming(PlayerGameplayEventType eventType)
        {
            switch (eventType)
            {
                case PlayerGameplayEventType.InstantAction:
                case PlayerGameplayEventType.ContinuousActionStarted:

                    AlternativeActionModule.StartProcess();
                    break;
                case PlayerGameplayEventType.ContinuousActionCancelled:
                case PlayerGameplayEventType.ContinuousActionEnded:
                    AlternativeActionModule.StopProcess();
                    break;
            }
        }
    }
}
using Scenes.SessionRework.Characters.Base.Models;
using Scenes.SessionRework.Scripts.Cameras.Controller.Interfaces;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.AimingState;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState;
using Scenes.SessionRework.Scripts.Player.View;
using Scenes.SessionRework.Scripts.Services.Replication.Models;
using UnityEngine;

namespace Scenes.SessionRework.Characters.Skadi.Abilities.Aiming
{
    public class SkadiAimingAbility : ICharacterAbility
    {
        public ushort AbilityId => (ushort)SkadiAbility.Aiming;

        private readonly ICameraController _cameraController;
        private readonly PlayerView _playerView;
        private readonly bool _isLocallyControlled;

        public SkadiAimingAbility(
            ICameraController cameraController,
            PlayerView playerView,
            bool isLocallyControlled)
        {
            _cameraController = cameraController;
            _playerView = playerView;
            _isLocallyControlled = isLocallyControlled;
        }

        public void Process(
            PlayerGameplayEventType eventType,
            CharacterEventSource source)
        {
            switch (eventType)
            {
                case PlayerGameplayEventType.InstantAction:
                case PlayerGameplayEventType.ContinuousActionStarted:
                    StartAiming();
                    break;

                case PlayerGameplayEventType.ContinuousActionCancelled:
                case PlayerGameplayEventType.ContinuousActionEnded:
                    StopAiming();
                    break;
            }
        }

        private void StartAiming()
        {
            if (_isLocallyControlled)
                _cameraController.TryMoveToState<AimingCameraState>();

            Debug.Log("Starting aiming");
            _playerView.SetAiming(true);
        }

        private void StopAiming()
        {
            if (_isLocallyControlled)
                _cameraController.TryMoveToState<FollowCameraState>();

            Debug.Log("Stop aiming");
            _playerView.SetAiming(false);
        }
    }
}
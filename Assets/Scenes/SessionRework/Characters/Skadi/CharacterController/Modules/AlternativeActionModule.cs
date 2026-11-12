using System;
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.Cameras.Controller.Interfaces;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.AimingState;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.AlternativeAction.Meta;
using Scenes.SessionRework.Scripts.Player.View;
using UnityEngine;

namespace Scenes.SessionRework.Characters.Skadi.CharacterController.Modules
{
    public class AlternativeActionModule:IAlternativeActionModule
    {
        private readonly ICameraController _cameraController;
        private PlayerView _playerView;
        
        public AlternativeActionModule(
            ICameraController cameraController,
            PlayerView playerView)
        {
            _cameraController = cameraController;
            _playerView = playerView;
        }
        
        public void StartProcess()
        {
            _cameraController.TryMoveToState<AimingCameraState>();
            
            _playerView.Animator.SetBool("IsAiming", true);
            _playerView.Animator.SetTrigger("StartAiming");

            _playerView.IsAiming = true;
        }

        public void UpdateProcess(float deltaTime)
        {
            
        }

        public void StopProcess()
        {
            _cameraController.TryMoveToState<FollowCameraState>();
            _playerView.Animator.SetBool("IsAiming", false);
            
            _playerView.IsAiming = false;

        }
    }
}
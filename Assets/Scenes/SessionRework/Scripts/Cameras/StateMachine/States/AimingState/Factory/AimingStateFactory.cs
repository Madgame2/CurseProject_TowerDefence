using System;
using Scenes.SessionRework.Scripts.Cameras.Components;
using Scenes.SessionRework.Scripts.Cameras.Interfaces;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;
using Scenes.SessionRework.Scripts.GameWorld.View;
using Scenes.SessionRework.Scripts.Player.Interfaces;
using Zenject;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.AimingState.Factory
{
    public class AimingStateFactory:StateFactoryModule<AimingCameraState>
    {
        [Inject] private readonly CameraView _cameraView;
        [Inject] private readonly IPlayableCharacterView  _playableCharacterView;
        [Inject] private readonly ICameraInput _cameraInput;

        public override CameraStateType CameraState => AimingCameraState.CameraState;

        public override AimingCameraState Create(CameraStateConfig stateConfig)
        {
            if (stateConfig is not AimingCameraConfig aimingCameraConfig)
            {
                throw new ArgumentException();
            }
            
            var playerView = _playableCharacterView.PlayerView;
            var cameraAnchors = playerView.DeffoultStateCameraAnchors;
            
            if(cameraAnchors==null)
                throw new Exception("Can't find CameraAnchors!");
            
            return new AimingCameraState(
                _cameraView,
                cameraAnchors,
                _cameraInput,
                aimingCameraConfig.DistanceMultiplier);
        }
    }
}
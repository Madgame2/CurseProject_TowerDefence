using System;
using Scenes.SessionRework.Scripts.Cameras.Components;
using Scenes.SessionRework.Scripts.Cameras.Interfaces;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;
using Scenes.SessionRework.Scripts.GameWorld.View;
using Scenes.SessionRework.Scripts.Player.Interfaces;
using Scenes.SessionRework.Scripts.Player.View;
using Zenject;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState.Factory
{
    public class FollowCharacterDeffoultStateFactory:StateFactoryModule<FollowCameraState>
    {
        [Inject] private readonly CameraView _cameraView;
        [Inject] private readonly IPlayableCharacterView  _playableCharacterView;
        [Inject] private readonly ICameraInput _cameraInput;
        
        public override CameraStateType CameraState => FollowCameraState.CameraState;

        public override FollowCameraState Create(CameraStateConfig stateConfig)
        {
            var playerView = _playableCharacterView.PlayerView;
            var cameraAnchors = playerView.DeffoultStateCameraAnchors;
            if(cameraAnchors ==null)
                throw new Exception("Can't find CameraAnchors!");
            
            return new FollowCameraState(
                _cameraView,
                cameraAnchors,
                _cameraInput);
        }
    }
}
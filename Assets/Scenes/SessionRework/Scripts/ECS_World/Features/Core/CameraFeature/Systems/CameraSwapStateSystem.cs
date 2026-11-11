using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.Cameras.Controller.Interfaces;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.AimingState;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.FollowCharacterDeffoultState;

namespace Scenes.SessionRework.Scripts.ECS_World.Systems.Input
{
    public class CameraSwapStateSystem:ISystem
    {
        public World World { get; set; }
        private ICameraController _cameraController;
        private readonly BaseInputActions _inputs;


        public CameraSwapStateSystem(World world, ICameraController cameraController, BaseInputActions inputs)
        {
            _cameraController = cameraController;
            World = world;
            _inputs = inputs;
        }

        public void OnAwake()
        {
            
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_inputs.SessionPlayer.RightClick.IsPressed())
            {
                _cameraController.TryMoveToState<AimingCameraState>();
            }
            else
            {
                _cameraController.TryMoveToState<FollowCameraState>();
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}
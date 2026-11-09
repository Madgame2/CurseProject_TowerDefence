using System;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Services.TypesMapper.Interfaces
{
    public interface ICameraStateMapper
    {
        Type Map(CameraStateType cameraState);
    }
}
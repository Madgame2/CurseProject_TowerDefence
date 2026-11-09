using System;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.DTO.Enums
{
    [Flags]
    public enum ActionFlags : byte
    {
        None = 0,
        JumpRequested = 1 << 0,
        AlternativeAction = 1 << 1
    }
}
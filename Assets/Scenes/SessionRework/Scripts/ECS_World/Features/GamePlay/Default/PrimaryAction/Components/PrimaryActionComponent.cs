using Scellecs.Morpeh;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.GamePlay.Default.PrimaryAction.Components
{
    public struct PrimaryActionComponent : IComponent
    {
        public bool WasPressed;
        public bool IsPressed;
        public bool WasReleased;
    }
}
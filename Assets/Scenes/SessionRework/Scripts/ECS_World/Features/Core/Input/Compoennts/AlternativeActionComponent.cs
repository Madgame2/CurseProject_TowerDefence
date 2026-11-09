using Scellecs.Morpeh;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.Input.Compoennts
{
    public struct AlternativeActionComponent : IComponent
    {
        public bool WasPressed;
        public bool IsPressed;
        public bool WasReleased;
    }
}
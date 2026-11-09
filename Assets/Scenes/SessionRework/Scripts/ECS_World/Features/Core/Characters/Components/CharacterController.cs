using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Meta;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters.Components
{
    public class CharacterController
    {
        public struct PlayerControllerComponent : IComponent
        {
            public CharacterControllerBase CharacterController;
        }
    }
}
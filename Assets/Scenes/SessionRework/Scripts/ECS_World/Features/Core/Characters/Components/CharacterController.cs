using Scellecs.Morpeh;
using Scenes.SessionRework.Characters.Base.Interfaces;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters.Components
{
    public class CharacterController
    {
        public struct PlayerControllerComponent : IComponent
        {
            public ICharacterGameplayEventHandler  Handler;
        }
    }
}
using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Components.Players;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Meta;

namespace Scenes.SessionRework.Scripts.ECS_World.Features.Core.Characters.Meta
{
    public interface ICharacterControllerFactoryModule
    {
        public CharacterControllerBase CreateController(World world, Scellecs.Morpeh.Entity playerEntity);
    }
}
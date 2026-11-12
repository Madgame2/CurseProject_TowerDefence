using Scenes.SessionRework.Characters.Base.Models;

namespace Scenes.SessionRework.Characters.Base.Interfaces
{
    public interface ICharacterGameplayEventHandler
    {
        void HandleEvent(CharacterGameplayEvent gameplayEvent);
    }
}
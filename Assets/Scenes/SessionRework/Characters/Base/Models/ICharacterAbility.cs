using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Characters.Base.Models
{
    public interface ICharacterAbility
    {
        ushort AbilityId { get; }

        void Process(
            PlayerGameplayEventType eventType,
            CharacterEventSource source);
    }
}
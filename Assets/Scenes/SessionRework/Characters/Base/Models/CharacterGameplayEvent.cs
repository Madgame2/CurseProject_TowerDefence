using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Characters.Base.Models
{
    public struct CharacterGameplayEvent
    {
        public CharacterEventSource Source;
        public CharacterEventKind Kind;

        public ushort Id;

        public PlayerGameplayEventType EventType;
    }
}
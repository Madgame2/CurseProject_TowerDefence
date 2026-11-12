using System.Collections.Generic;
using Scenes.SessionRework.Characters.Base.Interfaces;
using Scenes.SessionRework.Characters.Base.Models;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Characters.Base
{
    public abstract class CharacterControllerBase : ICharacterGameplayEventHandler
    {
        private readonly Dictionary<ushort, ushort> _inputBindings = new();
        private readonly Dictionary<ushort, ICharacterAbility> _abilities = new();

        protected void RegisterInput(
            CharacterInput input,
            ushort ability)
        {
            _inputBindings.Add(
                (ushort)input,
                ability);
        }
        
        protected void RegisterAbility(ICharacterAbility ability)
        {
            _abilities.Add(ability.AbilityId, ability);
        }
        
        public void HandleEvent(CharacterGameplayEvent gameplayEvent)
        {
            ushort abilityId;

            switch (gameplayEvent.Kind)
            {
                case CharacterEventKind.Input:
                    if (!_inputBindings.TryGetValue(gameplayEvent.Id, out abilityId))
                        return;
                    break;

                case CharacterEventKind.Ability:
                    abilityId = gameplayEvent.Id;
                    break;

                default:
                    return;
            }

            if (!_abilities.TryGetValue(abilityId, out var ability))
                return;

            ability.Process(
                gameplayEvent.EventType,
                gameplayEvent.Source);
        }
    }
}
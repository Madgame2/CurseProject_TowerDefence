using Common.systems.ScriptDirectorSystem.ActorsCasts.Actors;
using Common.systems.ScriptDirectorSystem.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common.systems.ScriptDirectorSystem.ActorsCasts
{
    public class ActorCast
    {
        private List<ActorParams> cast = new();

        public List<ActorParams> actorParams => cast;

        // Создаёт нового актёра или возвращает существующего по никнейму
        public ActorParams DefineActor(string actorNickName)
        {
            var existing = cast.FirstOrDefault(a => a.NickName == actorNickName);
            if (existing != null)
                return existing;

            var newActor = new ActorParams(actorNickName);
            cast.Add(newActor);
            return newActor;
        }

        // Метод для замены старого актёра новым наследником
        public void ReplaceActor(string actorNickName, ActorParams newActor)
        {
            for (int i = 0; i < cast.Count; i++)
            {
                if (cast[i].NickName == actorNickName)
                {
                    cast[i] = newActor;
                    return;
                }
            }

            // если не нашли, добавляем новый
            cast.Add(newActor);
        }

        // Получить актёра по никнейму
        public ActorParams GetActor(string actorNickName)
        {
            return cast.FirstOrDefault(a => a.NickName == actorNickName);
        }
    }
}

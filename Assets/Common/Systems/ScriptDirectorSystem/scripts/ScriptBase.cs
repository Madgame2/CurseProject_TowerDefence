using Common.systems.ScriptDirectorSystem.ActorsCasts;
using Common.systems.ScriptDirectorSystem.ActorsCasts.Actors;
using System;
using System.Xml.Linq;
using UnityEngine;

namespace Common.systems.ScriptDirectorSystem.Scripts
{
    public abstract class ScriptBase
    {
        private ActorContainer actors;


        public abstract void Actors(ref ActorCast cast);
        public abstract void Actions();

        public void SetActorsCast(ActorContainer actors)
        {
            this.actors = actors;
        }

        public T Actor<T>(string actorNickname) where T : IBackEndActor
        {
            var actor = actors.Get(actorNickname);

            if (actor is T typedActor)
                return typedActor;

            throw new InvalidCastException($"Actor '{actorNickname}' is not of type {typeof(T).Name}");
        }

        public T ActionActor<T>(string actorNickname) where T :  MonoBehaviour
        {
            var actor = actors.Get(actorNickname);

            if (actor is T typedActor)
                return typedActor;

            throw new InvalidCastException($"Actor '{actorNickname}' is not of type {typeof(T).Name}");
        }
    }
}

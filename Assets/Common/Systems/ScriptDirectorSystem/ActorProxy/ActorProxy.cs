using System.Dynamic;
using UnityEngine;

namespace Common.systems.ScriptDirectorSystem.Proxy
{
    public class ActorProxy : DynamicObject
    {
        private object actor;

        public ActorProxy(object actor)
        {
            this.actor = actor;
        }

        public override bool TryInvokeMember(
            InvokeMemberBinder binder,
            object[] args,
            out object result)
        {
            var type = actor.GetType();

            var method = ActorMethodCache.GetMethod(type, binder.Name);

            result = method.Invoke(actor, args);

            return true;
        }
    }
}

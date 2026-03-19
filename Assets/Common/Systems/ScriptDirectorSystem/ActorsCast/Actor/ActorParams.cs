using Common.systems.ScriptDirectorSystem.ActorsCasts.Actors.Types;
using System;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

namespace Common.systems.ScriptDirectorSystem.ActorsCasts.Actors
{
    public class ActorParams
    {
        protected string actorNickName;
        protected ActorRoleType actorRoleType;

        public string NickName { get => actorNickName; }
        public ActorRoleType RoleType { get => actorRoleType; }
        public ActorParams(string actorNickName)
        {
            this.actorNickName = actorNickName;
        }


        public BackedEndActorParams DefineLikeBackEndActor(ActorCast cast = null)
        {
            var newActor = new BackedEndActorParams(actorNickName);


            cast?.ReplaceActor(actorNickName, newActor);

            return newActor;
        }


        public ActionActorParams DefineLikeActionActor(ActorCast cast = null)
        {
            var newActor = new ActionActorParams(actorNickName);

            cast?.ReplaceActor(actorNickName, newActor);
            return newActor;
        }


        public class ActionActorParams : ActorParams
        {
            protected Type roleType;
            protected string gameObjectName;
            protected Transform parentTransform;
            protected string gameObjectTag;
            protected ActorResolvePolicy resolvePolicy;


            public string GameObjectTag => gameObjectTag;
            public string GameObjectName => gameObjectName;
            public Transform ParentTransform => parentTransform;
            public Type RoleType { get => roleType; }
            public ActorResolvePolicy ResolvePolicy { get { return resolvePolicy; } }

            public ActionActorParams(string actorNickName) : base(actorNickName)
            {
                this.actorRoleType = ActorRoleType.ActionActor;
            }

            public ActionActorParams defineRole<Role>() where Role : MonoBehaviour
            {
                roleType = typeof(Role);
                return this;
            }


            public ActionActorParams WithGameObjectName(string name)
            {
                gameObjectName = name;
                return this;
            }

            public ActionActorParams WithTag(string tag)
            {
                gameObjectTag = tag;
                return this;
            }

            public ActionActorParams WithParent(Transform parent)
            {
                parentTransform = parent;
                return this;
            }


            public ActionActorParams FindExistOrCreate()
            {
                resolvePolicy = ActorResolvePolicy.FindOrCreate;
                return this;
            }

            public ActionActorParams FindExist()
            {
                resolvePolicy = ActorResolvePolicy.FindExist;
                return this;
            }

            public ActionActorParams Create()
            {
                resolvePolicy = ActorResolvePolicy.Create;
                return this;
            }
        }

        public class BackedEndActorParams : ActorParams
        {
            protected Type roleType;

            protected ActorResolvePolicy resolvePolicy;

            public Type RoleType { get =>  roleType; }

            public ActorResolvePolicy ResolvePolicy { get { return resolvePolicy; } }

            public BackedEndActorParams(string actorNickName) : base(actorNickName)
            {
                this.actorRoleType = ActorRoleType.BackedEndActor;
            }

            public BackedEndActorParams defineRole<Role>() where Role : IBackEndActor
            {
                roleType = typeof(Role);
                return this;
            }

            public BackedEndActorParams FindExistOrCreate()
            {
                resolvePolicy = ActorResolvePolicy.FindOrCreate;
                return this;
            }

            public BackedEndActorParams FindExist()
            {
                resolvePolicy = ActorResolvePolicy.FindExist;
                return this;
            }

            public BackedEndActorParams Create()
            {
                resolvePolicy = ActorResolvePolicy.Create;
                return this;
            }


        }
    }
}

using Common.systems.ScriptDirectorSystem.ActorsCasts;
using Common.systems.ScriptDirectorSystem.ActorsCasts.Actors;
using Common.systems.ScriptDirectorSystem.ActorsCasts.Actors.Types;
using Common.systems.ScriptDirectorSystem.Scripts;
using UnityEngine;
using Zenject;
using static Common.systems.ScriptDirectorSystem.ActorsCasts.Actors.ActorParams;


namespace Common.systems.ScriptDirectorSystem
{
    public class ScriptDirector
    {
        private readonly DiContainer container;

        public ScriptDirector(DiContainer container)
        {
            this.container = container;
        }

        public void PlayScript<T>() where T : ScriptBase , new()
        {
            T script = new T();

            ActorCast cast = new ActorCast();
            script.Actors(ref cast);
            var actors = GetActors(cast);

            script.SetActorsCast(actors);
            script.Actions();
        }

        private object tryFindExistBackEnd(BackedEndActorParams param)
        {
            return container.TryResolve(param.RoleType);
        }

        private object tryCreateBackEndActor(BackedEndActorParams param)
        {
            return container.Instantiate(param.RoleType);
        }


        private object tryFindExistActionActor(ActionActorParams param)
        {
            if (param.RoleType == null)
                return null;

            object found = null;


            if (!string.IsNullOrEmpty(param.GameObjectTag))
            {
                GameObject objWithTag = GameObject.FindWithTag(param.GameObjectTag);
                if (objWithTag != null)
                    found = objWithTag.GetComponent(param.RoleType);
            }

            if (found == null && !string.IsNullOrEmpty(param.GameObjectName))
            {
                GameObject objByName = GameObject.Find(param.GameObjectName);
                if (objByName != null)
                    found = objByName.GetComponent(param.RoleType);
            }

            if (found == null)
            {
                found = UnityEngine.Object.FindFirstObjectByType(param.RoleType);
            }

            return found;
        }

        private object tryCreateActionActor(ActionActorParams param)
        {
            if (param.RoleType == null)
                return null;

            GameObject go;

            if (!string.IsNullOrEmpty(param.GameObjectName))
            {
                go = new GameObject(param.GameObjectName);
            }
            else
            {
                go = new GameObject(param.RoleType.Name);
            }


            if (!string.IsNullOrEmpty(param.GameObjectTag))
            {
                go.tag = param.GameObjectTag;
            }

            if (param.ParentTransform != null)
            {
                go.transform.SetParent(param.ParentTransform, false);
            }

            MonoBehaviour component = container.InstantiateComponent(param.RoleType, go) as MonoBehaviour;

            return component;
        }


        private object tryFindBackEndActor(ActorParams param)
        {
            BackedEndActorParams actor = (BackedEndActorParams)param;

            switch (actor.ResolvePolicy)
            {
                case ActorResolvePolicy.FindOrCreate:
                    {
                        var obj = tryFindExistBackEnd(actor);
                        return obj != null ? obj : tryCreateBackEndActor(actor);
                    }
                case ActorResolvePolicy.FindExist:
                    {
                        return tryFindExistBackEnd(actor);
                    }
                case ActorResolvePolicy.Create:
                default:
                    {
                        return tryCreateBackEndActor(actor);
                    }
            }
        }

        private object tryFindActionActor(ActorParams param)
        {
            ActionActorParams actionActor = (ActionActorParams)param;


            switch (actionActor.ResolvePolicy)
            {
                case ActorResolvePolicy.FindOrCreate:
                    {
                        var obj = tryFindExistActionActor(actionActor);
                        return obj != null ? obj : tryCreateActionActor(actionActor);
                    }
                case ActorResolvePolicy.FindExist:
                    {
                        return tryFindExistActionActor(actionActor);
                    }
                case ActorResolvePolicy.Create:
                default:
                    {
                        return tryCreateActionActor(actionActor);
                    }

            }
        }

        private ActorContainer GetActors(ActorCast cast)
        {
            ActorContainer result = new ActorContainer();
            foreach (var item in cast.actorParams)
            {
                switch (item.RoleType)
                {
                    case ActorRoleType.ActionActor:
                        {
                            var actor = tryFindActionActor(item);
                            result.Add(item.NickName, actor);
                        }
                        break;
                    case ActorRoleType.BackedEndActor:
                        {
                            var actor = tryFindBackEndActor(item);
                            result.Add(item.NickName, actor);
                        }
                        break;
                }
            }

            return result;
        }
    }
}

using Common.systems.ScriptDirectorSystem.Proxy;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.systems.ScriptDirectorSystem.ActorsCasts
{
    public class ActorContainer
    {
        private Dictionary<string, object> actors = new();

        public void Add(string name, object actor)
        {
            actors[name] = actor;
        }

        public object Get(string name)
        {
            if (!actors.TryGetValue(name, out var actor))
                throw new Exception($"Actor {name} not found");

            return actor;
        }
    }
}

using System.Collections.Generic;
using System;
using System.Reflection;

namespace Common.systems.ScriptDirectorSystem.Proxy
{
    public static class ActorMethodCache
    {
        private static Dictionary<(Type, string), MethodInfo> cache = new();

        public static MethodInfo GetMethod(Type type, string methodName)
        {
            var key = (type, methodName);

            if (!cache.TryGetValue(key, out var method))
            {
                method = type.GetMethod(methodName);

                if (method == null)
                    throw new Exception($"Method {methodName} not found in {type.Name}");

                cache[key] = method;
            }

            return method;
        }
    }
}

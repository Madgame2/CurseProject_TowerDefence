using GameServer.Domain.SessionWorld.Graphs.Base.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenes.SessionRework.Scripts.GameWorld.Graph.TreesGraph.Models;

namespace GameServer.Domain.SessionWorld.Graphs.Base
{
    public static class DataTypeRegistry
    {
        private static readonly Dictionary<GenericTypes, Type> _enumToType = new()
        {
            {GenericTypes.Bool, typeof(bool) },
            {GenericTypes.String, typeof(string)  },
            {GenericTypes.Int, typeof(int)  },
            {GenericTypes.Float, typeof(float)  },
            {GenericTypes.StaticTreeData, typeof(StaticTreeData) }
        };

        private static readonly Dictionary<Type, GenericTypes> _typeToEnum =
        _enumToType.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        public static Type GetType(GenericTypes typeName)
        {
            if (_enumToType.TryGetValue(typeName, out Type type))
                return type;

            throw new Exception($"Unknown GenericTypes enum: {typeName}");
        }

        public static GenericTypes GetEnum(Type type)
        {
            if (_typeToEnum.TryGetValue(type, out GenericTypes genericType))
                return genericType;

            throw new Exception($"Type {type.FullName} is not registered in DataTypeRegistry");
        }

        public static bool TryGetEnum(Type type, out GenericTypes genericType)
        {
            return _typeToEnum.TryGetValue(type, out genericType);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameServer.Domain.SessionWorld.Graphs.Base.Meta
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GenericTypes
    {
        None,
        String,
        Int,
        Float,
        Bool,
        StaticTreeData
    }
}

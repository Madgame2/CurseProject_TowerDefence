using System;
using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.Modules.Spawn.Modules.Exceptions
{
    public class NotFoundSpawnModuleException : Exception
    {
        public NotFoundSpawnModuleException(ObjectType objectType):
            base($"not found module to spawn object of type {objectType}"){}
    }
}
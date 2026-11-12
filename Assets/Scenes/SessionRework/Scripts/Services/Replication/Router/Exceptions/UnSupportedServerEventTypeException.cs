using System;
using Scenes.SessionRework.Scripts.Services.Replication.Events;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.Exceptions
{
    public class UnSupportedServerEventTypeException : Exception
    {
        public UnSupportedServerEventTypeException(EventType serverEventEventType) 
            : base($"{serverEventEventType.ToString()} not suppoerted type")
        { }
    }
}
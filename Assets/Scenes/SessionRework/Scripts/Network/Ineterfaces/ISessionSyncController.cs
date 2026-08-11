using System;

namespace Scenes.SessionRework.Scripts.Network.Ineterfaces
{
    public interface ISessionSyncController
    {
        void SubscribeToServerEvents();
        void SendClientReady();
    }
}
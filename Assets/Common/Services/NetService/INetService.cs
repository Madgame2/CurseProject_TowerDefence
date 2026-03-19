using Common.systems.ScriptDirectorSystem.ActorsCasts.Actors;
using UnityEngine;

namespace Common.Services.Net
{
    public interface INetService : IBackEndActor
    {
        void debug();
    }
}

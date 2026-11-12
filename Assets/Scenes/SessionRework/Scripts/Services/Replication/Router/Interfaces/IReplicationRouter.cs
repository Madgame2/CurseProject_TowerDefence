using Scenes.SessionRework.Scripts.Services.Replication.Models;

namespace Scenes.SessionRework.Scripts.Services.Replication.Router.Interfaces
{
    public interface IReplicationRouter
    {
        public void Route(ServerEvent serverEvent);
    }
}
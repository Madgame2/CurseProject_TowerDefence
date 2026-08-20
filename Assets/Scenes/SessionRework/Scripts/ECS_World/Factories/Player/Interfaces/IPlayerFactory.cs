using Scenes.SessionRework.Scripts.ECS_World.Factories.Player.Model;

namespace Scenes.SessionRework.Scripts.ECS_World.Factories.Interfaces
{
    public interface IPlayerFactory
    {
        Scellecs.Morpeh.Entity CreatePlayer(PlayerData data);
    }
}
using System.Threading.Tasks;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Player
{
    public interface IPlayerInputHandler
    {
        Task ProcessPlayerMovemnet(Vector3 direction);
        Task ProcessPlayerLook(Vector2 lookDelta);
    }
}

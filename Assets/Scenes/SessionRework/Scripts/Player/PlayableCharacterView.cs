using Scenes.SessionRework.Scripts.Player.Interfaces;
using Scenes.SessionRework.Scripts.Player.View;

namespace Scenes.SessionRework.Scripts.Player
{
    public class PlayableCharacterView:IPlayableCharacterView
    {
        public PlayerView PlayerView { get; set; }
    }
}
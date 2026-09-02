using Scellecs.Morpeh;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Players
{
    public struct PlayerComponent : IComponent
    {
        public string PlayerId;
    }
    
    public struct MyPlayerComponent: IComponent { }
    
    public struct CharacterViewComponent : IComponent
    {
        public Transform CharacterRoot;
        public Transform SpineBone;
    }
}
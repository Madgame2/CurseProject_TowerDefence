using Scellecs.Morpeh;
using Scenes.SessionRework.Scripts.ECS_World.Features.Core.Meta;
using Scenes.SessionRework.Scripts.Player.View;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.ECS_World.Components.Players
{
    public struct PlayerComponent : IComponent
    {
        public uint PlayerId;
        public PlayerView PlayerView;
    }
    
    public struct MyPlayerComponent: IComponent { }
    
    public struct CharacterViewComponent : IComponent
    {
        public Transform CharacterRoot;
        public Transform SpineBone;
    }

    public struct CharacterState : IComponent
    {
        
    }
}
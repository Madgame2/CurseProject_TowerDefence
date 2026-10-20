using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Enums;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model.Base;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes
{
    public class HitboxMeta: MonoBehaviour
    {
        [SerializeField]
        private HitboxShape _shape;

        [SerializeField]
        private HitboxShapes _data;
        
        
        public HitboxShape Shape => _shape;
        public HitboxShapes Data => _data;
    }
}
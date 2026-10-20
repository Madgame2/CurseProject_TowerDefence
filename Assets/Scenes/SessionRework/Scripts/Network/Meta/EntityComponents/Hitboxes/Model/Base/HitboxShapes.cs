using System;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Hitboxes.Model.Base
{
    [Serializable]
    public class HitboxShapes
    {
        public SphereHitboxData Sphere;
        public BoxHitboxData Box;
        public CapsuleHitboxData Capsule;
    }
}
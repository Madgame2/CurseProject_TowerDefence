using UnityEngine;

namespace Scenes.SessionRework.Scripts.Cameras.Components
{
    public class CameraAnchors: MonoBehaviour
    {
        [field: SerializeField]
        public Transform FollowAnchor { get; private set; }

        [field: SerializeField]
        public Transform LookAnchor { get; private set; }
        
        public Transform TransfromAnchor { get => this.transform; }
    }
}
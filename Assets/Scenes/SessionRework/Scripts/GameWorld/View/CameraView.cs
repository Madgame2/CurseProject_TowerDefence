using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.View
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private readonly Plane[] _frustumPlanes = new Plane[6];
        
        public Camera Camera
        {
            get => _camera;
        }

        public Plane[] Bounds 
        {
            get{
                GeometryUtility.CalculateFrustumPlanes(_camera, _frustumPlanes);
                return _frustumPlanes;
            }
        }
    }
}
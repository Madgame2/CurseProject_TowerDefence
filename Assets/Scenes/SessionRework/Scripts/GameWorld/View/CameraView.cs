using UnityEngine;

namespace Scenes.SessionRework.Scripts.GameWorld.View
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        private float _maxRayCastDistance = 200f;
        
        private readonly Plane[] _frustumPlanes = new Plane[6];
        
        public Vector3 LookAtPoint
        {
            get
            {
                Transform cameraTransform = _camera.transform;

                Ray ray = new Ray(
                    cameraTransform.position,
                    cameraTransform.forward);

                if (Physics.Raycast(
                        ray,
                        out RaycastHit hit,
                        _maxRayCastDistance))
                {
                    return hit.point;
                }

                return ray.origin + ray.direction * _maxRayCastDistance;
            }
        }

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

        public Transform Transform =>  _camera.transform;
    }
}
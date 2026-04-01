using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Lobby.NavSystem
{
    [ExecuteAlways]
    public class Path : MonoBehaviour
    {
        [SerializeField] private string _pathName;
        [SerializeField] private float _speed;
        public Transform[] Points;


        public float ExecSpeed { get => _speed; private set => _speed = value; }

        public string PathName { get => _pathName; private set => _pathName = value; }

        private void OnTransformChildrenChanged()
        {
            Points = GetAllPoints();
            Debug.Log(Points.Length);
        }

        private void OnDrawGizmos()
        {
            if (Points == null || Points.Length < 2) return;

            Gizmos.color = Color.green;

            for (int i = 0; i < Points.Length - 1; i++)
            {
                if (Points[i] != null && Points[i + 1] != null)
                {
                    Gizmos.DrawLine(Points[i].position, Points[i + 1].position);
                }
            }
        }

        private Transform[] GetAllPoints()
        {
            return GetComponentsInChildren<PathPoint>().Select(t=>t.transform).ToArray();
        }
    }
}
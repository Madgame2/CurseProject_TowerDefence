using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.Base;
using Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.SpawnPoint.DTO;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.SpawnPoint
{
    public class SpawnPointAnchor : NetComponentBase
    {
        [Header("Anchor Configuration")] 
        
        [SerializeField]
        private string _anchorName;
        
        [SerializeField]
        private Vector3 _spawnPoint;

        [SerializeField]
        private Vector3 _pivotPoint;
        
        [Header("Gizmos")]
        [SerializeField]
        private float _spawnPointRadius = 0.08f;

        [SerializeField]
        private float _pivotPointRadius = 0.1f;

        public Vector3 SpawnPoint => _spawnPoint;
        public Vector3 PivotPoint => _pivotPoint;
        
        
        
        public override object GetSerializationData()
        {
            return new SpawnPointAnchorDto
            {
                AnchorName = _anchorName,
                
                SpawnPoint = new Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.SpawnPoint.DTO.Vector3Dto(_spawnPoint),
                PivotPoint = new Scenes.SessionRework.Scripts.Network.Meta.EntityComponents.SpawnPoint.DTO.Vector3Dto(_pivotPoint)
            };
        }
        
        private void OnDrawGizmosSelected()
        {
            // Точка появления стрелы
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(
                transform.TransformPoint(_spawnPoint),
                _spawnPointRadius
            );

            // Точка вращения якоря
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(
                transform.TransformPoint(_pivotPoint),
                _pivotPointRadius
            );

            // Линия между pivot и spawn point
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(
                transform.TransformPoint(_pivotPoint),
                transform.TransformPoint(_spawnPoint)
            );
        }
    }
}
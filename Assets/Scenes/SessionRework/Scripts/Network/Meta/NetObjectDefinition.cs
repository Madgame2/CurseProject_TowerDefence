using Scenes.SessionRework.Scripts.Network.Meta.Enums;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Network.Meta
{
    public class NetObjectDefinition: MonoBehaviour
    {
        [SerializeField] private ObjectType _objectType;
        [SerializeField] private string _objectName;
        
        
        public ObjectType ObjectType => _objectType;
        public string ObjectName => _objectName;
    }
}
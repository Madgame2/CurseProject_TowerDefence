using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.SessionRework.Scripts.Player.View
{
    public class PlayerView: MonoBehaviour
    {
        [SerializeField] private CameraSettingsDefinition _cameraSettingsDefinition;
        [SerializeField] private Transform _cameraStartPosition;
        [SerializeField] private Transform _cameraTargetLook;
        [SerializeField] private Transform _playerLooTarget;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _spineBone;
        
        private Entity _entity;

        public bool HasEntity {get; private set; }
        public Entity Entity => _entity;
        
        public Transform PlayerLooTarget => _playerLooTarget;
        public Transform SpineBone => _spineBone;
        public Transform CameraStartPosition { get =>  _cameraStartPosition; }
        public Transform CameraTargetLook { get =>  _cameraTargetLook; }
        public CameraSettingsDefinition  CameraSettingsDefinition { get => _cameraSettingsDefinition; }
        public Animator Animator => _animator;

        public void LinkEntity(Entity entity)
        {
            _entity =  entity;
            HasEntity = true;
        }
    }
}
using System;
using Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Meta;
using UnityEngine;

namespace Scenes.SessionRework.Scripts.Cameras.StateMachine.States.Base
{
    [Serializable]
    public class CameraStateTransitionConfig
    {
        [SerializeField]
        private CameraStateConfig _from;

        [SerializeField]
        private CameraStateConfig _to;

        public CameraStateConfig From => _from;
        public CameraStateConfig To => _to;
    }
}
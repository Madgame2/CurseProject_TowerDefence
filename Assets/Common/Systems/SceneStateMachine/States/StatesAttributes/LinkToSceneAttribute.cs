using Common.Services.SceneServices.Scenes;
using System;
using UnityEngine;

namespace Common.systems.SceneStates.States.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LinkToSceneAttribute : Attribute
    {
        public Type SceneType { get; }

        public LinkToSceneAttribute(Type sceneType)
        {
            if (!typeof(SceneBase).IsAssignableFrom(sceneType))
            {
                throw new ArgumentException($"{sceneType} must inherit from SceneBase");
            }

            SceneType = sceneType;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.systems.MainThread
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> _actions = new Queue<Action>();

        public void Run(Action action)
        {
            lock (_actions)
            {
                _actions.Enqueue(action);
            }
        }

        private void Update()
        {
            while (true)
            {
                Action action = null;

                lock (_actions)
                {
                    if (_actions.Count == 0)
                        break;

                    action = _actions.Dequeue();
                }

                action?.Invoke();
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
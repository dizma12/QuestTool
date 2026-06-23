using QuestMaker.Domain;
using QuestMaker.Runtime.Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestMaker.Runtime.Events
{
    [DefaultExecutionOrder(-19)] // -20 Reference Manager
    public class GameEventManager : MonoBehaviour, IGameReference
    {
        private static GameEventManager instance = null;

        private Dictionary<Type, CustomEventBus> _eventBuses = null;


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            _eventBuses = new();

            if (!SubscribeSelf())
                ConsoleLogger.LogWarning(this, $"Failed to subscribe self to ReferenceManager");
        }

        private void OnDisable()
        {
            UnsubscribeSelf();
        }

        public bool RemoveBus<T>() where T : CustomEventBus
        {
            Type type = typeof(T);
            if (_eventBuses.ContainsKey(type))
            {
                _eventBuses.Remove(type);
                return true;
            }
            return false;
        }

        private T CreateBus<T>() where T : CustomEventBus, new()
        {
            Type type = typeof(T);
            if (_eventBuses.ContainsKey(type))
                return _eventBuses[type] as T;

            T bus = new();
            _eventBuses.Add(type, bus);
            //ConsoleLogger.Log(this, $"Created new Event Bus of type {bus.GetType().Name}");
            return bus;
        }

        public T RequestBus<T>() where T : CustomEventBus, new()
        {
            Type type = typeof(T);

            if (_eventBuses.ContainsKey(type))
                return _eventBuses[type] as T;
            else
                return CreateBus<T>();

        }


        private bool SubscribeSelf()
           => ReferenceManager.Instance.SubScribeReference<GameEventManager>(this);

        private bool UnsubscribeSelf()
        {
            if (ReferenceManager.Instance != null)
                return ReferenceManager.Instance.UnsubscribeReference<Player>();

            return false;
        }
    }
}

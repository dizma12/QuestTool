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

        private bool SubscribeSelf()
           => ReferenceManager.Instance.SubScribeReference<GameEventManager>(this);

        private bool UnsubscribeSelf()
           => ReferenceManager.Instance.UnsubscribeReference<GameEventManager>();

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

            CreateBus<GameEventBus>();
            CreateBus<QuestEventBus>();
        }

        private void OnDisable()
        {
            UnsubscribeSelf();
        }

        public bool AddBus<T>(T bus) where T : CustomEventBus
        {
            if (bus == null)
                return false;

            Type type = bus.GetType();

            if (_eventBuses.ContainsKey(type))
                return false;

            _eventBuses.Add(type, bus);
            return true;
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

        public T CreateBus<T>() where T : CustomEventBus, new()
        {
            Type type = typeof(T);
            if (_eventBuses.ContainsKey(type))
                return _eventBuses[type] as T;

            T bus = new();
            _eventBuses.Add(type, bus);
            return bus;
        }

        public T GetBus<T>() where T : CustomEventBus
        {
            Type type = typeof(T);

            if (_eventBuses.ContainsKey(type))
                return _eventBuses[type] as T;

            ConsoleLogger.LogWarning(this, $"GetBus<>: Could not find bus of type= {type}");
            return default;
        }
    }
}

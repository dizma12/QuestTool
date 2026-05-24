using QuestMaker.Runtime.Events.Handlers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace  QuestMaker.Runtime.Game.Events
{
    [DefaultExecutionOrder(-19)]
    public class GameEventManager : MonoBehaviour, IGameReference
    {
        private static GameEventManager instance = null;

        private Dictionary<Type, CustomEventHandler> eventHandlers = null;

        public bool SubscribeSelf()
           => ReferenceManager.Instance.SubScribeReference<GameEventManager>(this);

        public bool UnsubscribeSelf()
           => ReferenceManager.Instance.UnsubscribeReference<GameEventManager>();
        

        private void Awake()
        {
            if (instance == null || instance != this)
                Destroy(gameObject);

            instance = this;

            eventHandlers = new();

            if(!SubscribeSelf())
            {
                Debug.LogWarning($"[GameEventManager] Failed to subscribe self to ReferenceManager");
            }

            CreateEventHandler<GameEventHandler>();
            CreateEventHandler<ResourcesEventHandler>();
        }

        private void OnDisable()
        {
            UnsubscribeSelf();
        }
        public bool AddEventHandler<T>(T handler) where T : CustomEventHandler
        {
            if(handler == null)
                return false;

            Type type = handler.GetType();

            if(eventHandlers.ContainsKey(type))
                return false;

            eventHandlers.Add(type, handler);
            Debug.Log($"[GameEventManager] Added Handler of type {type}");
            return true;
        }
        public bool RemoveEventHandler<T>() where T : CustomEventHandler
        {
            Type type = typeof(T);
            if(eventHandlers.ContainsKey(type))
            {
                eventHandlers.Remove(type);
                return true;
            }
            return false;
        }
        public T CreateEventHandler<T>() where T : CustomEventHandler, new()
        {
            Type type = typeof(T);
            if (eventHandlers.ContainsKey(type))
                return eventHandlers[type] as T;

            T handler = new();
            eventHandlers.Add(type, handler);
            Debug.Log($"[GameEventManager].CreateEventHandler() Creating New Handler of type {type}");

            return handler;
        }
        public T GetEventHandler<T>() where T : CustomEventHandler
        {
            Type type = typeof(T);

            if( eventHandlers.ContainsKey(type))
                return eventHandlers[type] as T;


            Debug.LogWarning($"[GameEventManager].GetEventHandler<>: Could not find handler of type= {type}");
            return default;
            
        }

        public void EvetCryOut()
        {
            Debug.Log("[GameEventManager] Executing from external call");
        }
    }
}

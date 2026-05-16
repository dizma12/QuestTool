using System;
using System.Collections.Generic;

using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    [DefaultExecutionOrder(-20)]
    public class ReferenceManager : MonoBehaviour
    {
        public static ReferenceManager Instance;
        private Dictionary<Type, IGameReference> references = null;

        private void Awake()
        {

            if (Instance != null || Instance != this)
            {
                Destroy(gameObject);
            }

            Instance = this;

            references ??= new Dictionary<Type, IGameReference>();
        }

        public bool SubScribeReference<T>(IGameReference reference) where T : IGameReference
        {
            Type type = typeof(T);

            if(reference == null)
                return false;

            if (references.ContainsKey(type))
                return false;

            references.Add(type, reference);
            //Debug.Log($"Added manager of type: {type}");
            return true;

        }   

        public bool UnsubscribeReference<T>() where T : IGameReference
        {
            Type type = typeof(T);
            if(references.ContainsKey(type))
            {
                references.Remove(type);
                return true;
            }
            return false;
        }


        public T GetReference<T>() where T : IGameReference
        {
            if(references.TryGetValue(typeof(T), out IGameReference reference))
            {
                return (T)reference;
            }
            return default;
        }
    }
}

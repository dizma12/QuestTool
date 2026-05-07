using System;
using UnityEngine;

namespace Assets.Scripts.QuestMaker.Game.Events
{
    public class GlobalEventManager : MonoBehaviour
    {
        public static GlobalEventManager Instance { get; private set; }

        public QuestEvents QuestEvent { get; private set; }
            
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"[GlobalEventManager] Cannot have more than 1 instances of Global Event Manager");
            }
            Instance = this;

            InitializeEvents();
        }


        private void InitializeEvents()
        {
            QuestEvent = new();
        }
    }
}

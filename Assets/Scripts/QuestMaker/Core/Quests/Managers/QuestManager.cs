using QuestMaker.Core.Quests;
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Game.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace QuestMaker.Core
{
    public class QuestManager : MonoBehaviour, IGameReference
    {
        
        private Dictionary<string, Quest> _questMap = null;


        private void Awake()
        {
            LoadQuestMap();

            if (!SubscribeSelf())
                Debug.LogError($"[{GetType()}] Failed to Subscribe self on ReferenceManager");
        }

        private void OnDisable()
        {
            UnsubscribeSelf();
        }

        public Quest GetQuestByID(string id)
        {
            Quest q = _questMap[id];
            if (q == null )
            {
                Debug.LogError($"[{GetType()}] Quest ID not Found");
            }
            return q;
        }

        #region Helpers


        private void LoadQuestMap()
        {
            if (_questMap == null)
            {
                QuestLoader questLoader = new();
                _questMap ??= questLoader.CreateQuestMap();

            }
            if (_questMap == null)
                throw new InvalidOperationException($"[{GetType()}] Failed to Load quest map from QuestLoader");
        }

        private bool SubscribeSelf()
            => ReferenceManager.Instance.SubScribeReference<QuestManager>(this);

        private bool UnsubscribeSelf()
           => ReferenceManager.Instance.UnsubscribeReference<QuestManager>();
        #endregion
    }
}

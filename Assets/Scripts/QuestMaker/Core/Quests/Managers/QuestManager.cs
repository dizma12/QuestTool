using QuestMaker.Runtime.Quests;
using QuestMaker.Runtime.Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using QuestMaker.Runtime.Events;
using QuestMaker.Domain.Events;
using QuestMaker.Domain;
namespace QuestMaker.Runtime
{
    [DefaultExecutionOrder(-18)] //-20 Reference Manager, -19 GameEventManager
    public class QuestManager : MonoBehaviour, IGameReference
    {
        
        private Dictionary<string, Quest> _questMap = null;

        public IReadOnlyDictionary<string, Quest> QuestMap => _questMap;
        private void Awake()
        {
            LoadQuestMap();

            if (!SubscribeSelf())
                ConsoleLogger.LogError(this,"Failed to Subscribe self on ReferenceManager");
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
                ConsoleLogger.LogError(this, "Quest ID not Found");
            }
            return q;
        }

        #region Helpers


        private void LoadQuestMap()
        {
            if (_questMap == null)
            {
                IQuestEventSource eventbus = ReferenceManager.Instance.GetReference<GameEventManager>().GetBus<GameEventBus>();

                QuestLoader questLoader = new(eventbus);
                _questMap = questLoader.CreateQuestMap();

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

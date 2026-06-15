using QuestMaker.Runtime.Quests;
using QuestMaker.Runtime.Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using QuestMaker.Runtime.Events;
using QuestMaker.Domain.Events;
using QuestMaker.Domain;
using QuestMaker.Domain.Quests;
using System.Linq;

namespace QuestMaker.Runtime
{
    [DefaultExecutionOrder(-18)] //-20 Reference Manager, -19 GameEventManager
    public class QuestManager : MonoBehaviour, IGameReference
    {

        private Dictionary<string, Quest> _questMap = null;
        private IQuestEventSource _eventBus = null;
        private Player _player = null;
        public IReadOnlyDictionary<string, Quest> QuestMap => _questMap;
        private void Awake()
        {
            LoadQuestMap();

            if (!SubscribeSelf())
                ConsoleLogger.LogError(this, "Failed to Subscribe self on ReferenceManager");
        }

        private void Start()
        {
            CheckAllPrerequisites();
        }

        private void OnDisable()
        {
            UnsubscribeSelf();
        }

        public Quest GetQuestByID(string id)
        {
            if(!_questMap.TryGetValue(id, out Quest q))
                ConsoleLogger.LogError(this, "Quest ID not Found");
            return q;
        }

        /// <summary>
        /// Checks Prerequisites for all quests.
        /// </summary>
        public void CheckAllPrerequisites()
        {
            var missingReq = _questMap.Values.Where(q => q.Status == QuestStatus.MISSING_REQUIRMENTS);

            foreach (Quest quest in missingReq)
            {
                CheckQuestPrerequisites(quest);
            }
        }

        public void CheckQuestPrerequisites(Quest quest)
        {
            if (quest.Status != QuestStatus.MISSING_REQUIRMENTS) return;

            if (PrereqsMet(quest))
                quest.SetQuestStatus(QuestStatus.CAN_START);
        }

        public bool PrereqsMet(Quest quest)
        {
            PrerequisiteData prereq = quest._prerequisites;
            if (prereq == null) return true;

            _player ??= ReferenceManager.Instance.GetReference<Player>();
            if (_player == null)
            {
                ConsoleLogger.LogError(this, "Failed to retrieve Player Reference");
                return false;
            }

            if (_player.Level < prereq.Level)
                return false;

            foreach (ItemStack required in prereq.Items)
                if (_player.Inventory.GetItemCount(required.Item) < required.Amount)
                    return false;

            foreach (string questId in prereq.Quests)
                if (!_questMap.TryGetValue(questId, out Quest requiredQuest) || !requiredQuest.IsFinished)
                    return false;

            return true;
        }

        #region Helpers


        private void LoadQuestMap()
        {
            if (_questMap == null)
            {
                _eventBus ??= ReferenceManager.Instance.GetReference<GameEventManager>().RequestBus<GameEventBus>();

                QuestLoader questLoader = new(_eventBus);
                _questMap = questLoader.CreateQuestMap();

            }
            if (_questMap == null)
                throw new InvalidOperationException($"[{GetType()}] Failed to Load quest map from QuestLoader");
            else
                ConsoleLogger.Log(this, $"Loaded {_questMap.Count} quests");
        }

        private bool SubscribeSelf()
            => ReferenceManager.Instance.SubScribeReference<QuestManager>(this);

        private bool UnsubscribeSelf()
        {
            if (ReferenceManager.Instance != null)
                return ReferenceManager.Instance.UnsubscribeReference<Player>();

            return false;
        }
        #endregion
    }
}

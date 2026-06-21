using QuestMaker.Domain;
using QuestMaker.Domain.Events;
using QuestMaker.Domain.Quests;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace QuestMaker.Runtime
{
    [DefaultExecutionOrder(-18)] //-20 Reference Manager, -19 GameEventManager
    public class QuestManager : MonoBehaviour, IGameReference
    {

        private Dictionary<string, Quest> _questMap = null;
        private IQuestEventSource _gameEventBus = null;
        private QuestEventBus _questEventBus = null;

        private Player _player = null;
        public IReadOnlyList<Quest> ActiveQuests => _questMap.Values.Where(q => q.Status == QuestStatus.IN_PROGRESS).ToList();
        public IReadOnlyDictionary<string, Quest> QuestMap => _questMap;
        private void Awake()
        {
            if (!SubscribeSelf())
                ConsoleLogger.LogError(this, "Failed to Subscribe self on ReferenceManager");

            GameEventManager mng = ReferenceManager.Instance.GetReference<GameEventManager>();

            if (mng == null)
            {
                ConsoleLogger.LogError(this, "Failed to find reference of type GameEventManager");
                return;
            }

            _gameEventBus = mng.RequestBus<GameEventBus>();
            _questEventBus = mng.RequestBus<QuestEventBus>();

            LoadQuestMap();

           
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
            if(!_questMap.TryGetValue(id, out Quest quest))
                ConsoleLogger.LogError(this, "Quest ID not Found.");
            return quest;
        }

        public bool TryStartQuest(string questID)
        {
            if (string.IsNullOrEmpty(questID))
            {
                ConsoleLogger.LogError(this, "The questID you are trying to start is null.");
                return false;
            }
            if (!_questMap.TryGetValue(questID, out Quest quest))
            {
                ConsoleLogger.LogError(this, "QuestMap doesnt contain the questID, try creating it first with CreateQuest().");
                return false;
            }
            
            if(CheckQuestPrerequisites(quest))
            {
                quest.Start();
                quest.CanFinish += HandleQuestCanFinish;
                quest.Completed += HandleQuestCompleted;

                _questEventBus.FireQuestStarted(quest);
                return true;
            }    
            return false;
        }
        private void HandleQuestCanFinish(Quest quest)
        {
            _questEventBus.FireQuestCanFinish(quest);
        }
        private void HandleQuestCompleted(Quest quest)
        {
            _questEventBus.FireQuestCompleted(quest);
        }

        public bool TryTurnInQuest(string questID)
        {
            if (string.IsNullOrEmpty(questID))
            {
                ConsoleLogger.LogError(this, "The questID you are trying to turn in is null.");
                return false;
            }
            if (!_questMap.TryGetValue(questID, out Quest quest))
            {
                ConsoleLogger.LogError(this, "QuestMap doesnt contain the questID.");
                return false;
            }
            if (quest.Status != QuestStatus.CAN_FINISH) return false;

            quest.Complete();
            _questEventBus.FireQuestCompleted(quest);
            return true;
        }

        /// <summary>
        /// Checks if the quest already exists on questMap else creates a new and add it.
        /// </summary>
        /// <param name="questSO"></param>
        /// <param name="quest"></param>
        /// <returns></returns>
        public bool CreateQuest(QuestSO questSO, out Quest quest)
        {
            quest = null;
            if (questSO == null)
            {
                ConsoleLogger.LogError(this, "The quest you are trying to create is null.");
                return false;
            }

            if(_questMap.TryGetValue(questSO.ID, out quest))
                return true;
            else
            {
                quest = new(questSO, _gameEventBus);
                _questMap.Add(questSO.ID, quest);
                return true;
            }
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

        /// <summary>
        /// Checks Prerequisites for Quest.
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public bool CheckQuestPrerequisites(Quest quest)
        {
            if (quest == null)
            {
                ConsoleLogger.LogError(this, "The quest you are trying to check is null.");
                return false;
            }

            //If can start already just return true.
            if(quest.Status == QuestStatus.CAN_START) return true;

            //if its not on State Missing Req (In_progress,Can_Finish, Finished) return false.
            else if (quest.Status != QuestStatus.MISSING_REQUIRMENTS) return false;

            if (PrereqsMet(quest))
            {
                quest.SetQuestStatus(QuestStatus.CAN_START);
                return true;
            }
            return false;
        }

        private bool PrereqsMet(Quest quest)
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
                

                QuestLoader questLoader = new(_gameEventBus);
                _questMap = questLoader.CreateQuestMap();

            }
            if (_questMap == null)
                throw new InvalidOperationException($"[{GetType().Name}] Failed to Load quest map from QuestLoader");
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

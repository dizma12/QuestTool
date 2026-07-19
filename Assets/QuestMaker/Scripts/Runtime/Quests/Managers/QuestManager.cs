using QuestMaker.Domain;
using QuestMaker.Domain.Events;
using QuestMaker.Domain.Quests;
using QuestMaker.Domain.Steps;
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
        private PlayerEventBus _playerEventBus = null;

        private Player _player = null;
        private InventoryManager _inventoryManager = null;
        public IReadOnlyList<Quest> ActiveQuests => _questMap.Values.Where(q => q.Status == QuestStatus.IN_PROGRESS).ToList();
        public IReadOnlyDictionary<string, Quest> QuestMap => _questMap;
        private void Awake()
        {
            if (!SubscribeSelf())
                ConsoleLogger.LogError(this, "Failed to Subscribe self on ReferenceManager");

            //Events
            GameEventManager mng = ReferenceManager.Instance.RequestReference<GameEventManager>();
            if (mng == null)
            {
                ConsoleLogger.LogError(this, "Failed to find reference of type GameEventManager");
                return;
            }
            _gameEventBus = mng.RequestBus<GameEventBus>();
            _questEventBus = mng.RequestBus<QuestEventBus>();
            _playerEventBus = mng.RequestBus<PlayerEventBus>();

            if (_playerEventBus == null)
            {
                ConsoleLogger.LogError(this, "Failed to retrieve Player Event Bus Reference");
                return;
            }
            _playerEventBus.PlayerLevelChanged += CheckAllPrerequisites;

            //Initialize
            LoadQuestMap();
            SetQuestChains();
            
        }

        private void Start()
        {
            //Player
            _player = ReferenceManager.Instance.RequestReference<Player>();
            if (_player == null)
            {
                ConsoleLogger.LogError(this, "Failed to retrieve Player Reference");
                return;
            }

            _inventoryManager = ReferenceManager.Instance.RequestReference<InventoryManager>();
            if (_inventoryManager == null)
            {
                ConsoleLogger.LogError(this, "Failed to retrieve InventoryManager Reference");
                return;
            }

            CheckAllPrerequisites();
        }
        private void OnDisable()
        {
            UnsubscribeSelf();

            if (_playerEventBus != null)
                _playerEventBus.PlayerLevelChanged -= CheckAllPrerequisites; 
        }

        public Quest GetQuestByID(string id)
        {
            if(!_questMap.TryGetValue(id, out Quest quest))
                ConsoleLogger.LogError(this, "Quest ID not Found.");
            return quest;
        }

        /// <summary>
        /// Quest giver callback for assinging Hand-ins
        /// </summary>
        /// <param name="giverGuid"></param>
        /// <returns></returns>
        public IReadOnlyList<Quest> GetHandInQuests(string giverGuid)
        {
            List<Quest> result = new();
            if (string.IsNullOrEmpty(giverGuid)) return result;

            result = _questMap.Values.Where(q => q != null && q.HandInGiverGuid.Equals(giverGuid)).ToList();

            return result;
        }

        /// <summary>
        /// Quest Giver callback for assigning Turn-ins.
        /// </summary>
        /// <param name="giverGuid"></param>
        /// <returns></returns>
        public IReadOnlyList<Quest> GetTurnInQuests(string giverGuid)
        {
            List<Quest> result = new();
            if (string.IsNullOrEmpty(giverGuid)) return result;

            result = _questMap.Values.Where(q => q != null && q.TurnInGiverGuid.Equals(giverGuid)).ToList();

            return result;
        }

        /// <summary>
        /// Re-checks prerequisites. Subs to quest-specific events. Fires Quest Started Event.
        /// </summary>
        /// <param name="questID"></param>
        /// <returns></returns>
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
                quest.ObjectiveChanged += HandleObjectiveChanged;

                _questEventBus.FireQuestStarted(quest);
                HandleObjectiveChanged(quest);
                return true;
            }
            return false;
        }

        private void HandleQuestCanFinish(Quest quest)
        {
            _questEventBus.FireQuestCanFinish(quest);
            quest.CanFinish -= HandleQuestCanFinish;

            //if for some reason Turn-in giver was not set auto-complete quest.
            if(string.IsNullOrEmpty(quest.TurnInGiverGuid))
                TryTurnInQuest(quest.ID);
        }

        /// <summary>
        /// Callback for when an Objective of a quest changes. Also Syncs items with inventory on Item related Quests.
        /// </summary>
        /// <param name="quest"></param>
        private void HandleObjectiveChanged(Quest quest)
        {
            _questEventBus.FireQuestObjectiveChanged(quest);
            SyncQuestWithInventory(quest);
        }

        /// <summary>
        /// Checks inventory manager for the required Items if a quest is Item related.
        /// </summary>
        /// <param name="quest"></param>
        private void SyncQuestWithInventory(Quest quest)
        {
            if (_inventoryManager == null) return;

            foreach (QuestStep step in quest.CurrentSteps)
            {
                if (step is ItemQuestStep itemStep)
                    itemStep.CheckInventoryForExisting(_inventoryManager.Inventory);
            }
        }

        /// <summary>
        /// Completes the quest. Fires Quest completion Event. Also checks prerequisite for next-in-chain quests if any.
        /// </summary>
        /// <param name="questID"></param>
        /// <returns></returns>
        public bool TryTurnInQuest(string questID)
        {
            if (string.IsNullOrEmpty(questID))
            {
                ConsoleLogger.LogError(this, "The questID you are trying to turn in is null.");
                return false;
            }
            if (!_questMap.TryGetValue(questID, out Quest quest))
            {
                ConsoleLogger.LogError(this, $"QuestMap doesnt contain the questID: {questID}.");
                return false;
            }
            if (quest.Status != QuestStatus.CAN_FINISH) return false;

            quest.Complete();
            quest.ObjectiveChanged -= HandleObjectiveChanged;

            _questEventBus.FireQuestCompleted(quest);

            //checks preqs for next Quest in chain.
            if (quest.NextInChain != null && quest.NextInChain.Count > 0)
            {
                foreach(string next in quest.NextInChain)
                {
                    CheckQuestPrerequisites(GetQuestByID(next));
                }
            }

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
            _questEventBus.FirePrerequisitesChanged();
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

                //if for some reason givers were not set auto-start quest
                if(string.IsNullOrEmpty(quest.HandInGiverGuid))
                    TryStartQuest(quest.ID);

                return true;
            }
            return false;
        }

        private bool PrereqsMet(Quest quest)
        {
            PrerequisiteData prereq = quest.Prerequisites;
            if (prereq == null) return true;

            if (_player.Level < prereq.Level)
                return false;

            foreach (ItemStack required in prereq.Items)
                if (_inventoryManager.Inventory.GetItemCount(required.Item) < required.Amount)
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
        private void SetQuestChains()
        {
            foreach(Quest quest in _questMap.Values)
            {
                //Sets the next quest in chain
                if (quest.Prerequisites != null && quest.Prerequisites.Quests != null && quest.Prerequisites.Quests.Count > 0)
                {
                    foreach (string q in quest.Prerequisites.Quests)
                    {
                        Quest required = GetQuestByID(q);

                        if(required == null) continue;

                        required.SetNextInChain(quest.ID);

                        ConsoleLogger.Log(this, $"Quest {quest.ID} was set as next in chain for quest {q} ");
                    }
                }
            }    
        }
        private bool SubscribeSelf()
            => ReferenceManager.Instance.SubScribeReference<QuestManager>(this);

        private bool UnsubscribeSelf()
        {
            if (ReferenceManager.Instance != null)
                return ReferenceManager.Instance.UnsubscribeReference<QuestManager>();

            return false;
        }
        #endregion
    }
}

using QuestMaker.Domain;
using System;
using QuestMaker.Runtime.Quests;
using System.Collections.Generic;
using UnityEngine;
using QFSW.QC;
using System.Linq;
using QuestMaker.Domain.Interactions;
using QuestMaker.Domain.Quests;
namespace QuestMaker.Runtime.Game
{
    internal class QuestGiver : MonoBehaviour, IInteractable
    {
        [SerializeField] private QuestGiverData data = null;
        public string ID => data.ID;
        public IReadOnlyList<QuestSO> Quests => data.HandInQuests;
        
        private QuestManager _questManager;

        private void Start()
        {
            if(data == null || string.IsNullOrEmpty(data.ID) || data.HandInQuests == null)
                throw new ArgumentException("Invalid QuestGiverData");

            _questManager = ReferenceManager.Instance.GetReference<QuestManager>();

            if (_questManager == null)
                ConsoleLogger.LogError(this, "QuestManager reference not found");
           
        }

        [Command("Quest-giver", MonoTargetType.All)]
        public void CheckQuests()
        {
            var x = RefreshQuests();

            if (x.Count <= 0)
                ConsoleLogger.Log(this, $"{ID}: 0 / {Quests.Count} quests are available now");
            else
            {
                foreach (var item in Quests)
                {
                    ConsoleLogger.Log(this, $"{ID}: {item.ID} is available for pickup");
                }
            }
        }


        public IReadOnlyList<Quest> RefreshQuests()
        {
            List<Quest> quests = new();

            foreach (QuestSO so in Quests)
            {
                Quest quest = _questManager.GetQuestByID(so.ID);
                if (quest == null) continue;

                _questManager.CheckQuestPrerequisites(quest);
                quests.Add(quest);
            }

            return quests;
        }

        public void Interact(GameObject other)
        {
            ConsoleLogger.Log(this, $"Was interacted with {ID}!");

            if (TryTurnIn()) return;
            TryHandIn();
        }

        private bool TryTurnIn()
        {
            foreach (QuestSO so in data.TurnInQuests)
            {
                Quest quest = _questManager.GetQuestByID(so.ID);
                if (quest == null || quest.Status != QuestStatus.CAN_FINISH) continue;

                if (_questManager.TryTurnInQuest(quest.ID))
                {
                    ConsoleLogger.Log(this, $"{ID}: turned in {quest.ID}");
                    return true;
                }
            }
            return false;
        }

        private bool TryHandIn()
        {
            foreach (QuestSO so in data.HandInQuests)
            {
                Quest quest = _questManager.GetQuestByID(so.ID);
                if (quest == null || quest.Status != QuestStatus.CAN_START) continue;

                if (_questManager.TryStartQuest(quest.ID))
                {
                    ConsoleLogger.Log(this, $"{ID}: handed in {quest.ID}");
                    return true;
                }
            }
            return false;
        }
    }
}

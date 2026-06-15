using QuestMaker.Domain;
using QuestMaker.Runtime.Quests;
using System.Collections.Generic;
using UnityEngine;
using System;
using QFSW.QC;
namespace QuestMaker.Runtime.Game
{
    internal class QuestGiver : MonoBehaviour
    {
        [SerializeField] private string _npcId = string.Empty;
        [SerializeField] private List<QuestSO> _quests = new();

        private QuestManager _questManager;

        private void Start()
        {
            _questManager = ReferenceManager.Instance.GetReference<QuestManager>();

            if (_questManager == null)
                ConsoleLogger.LogError(this, "QuestManager reference not found");


            if (string.IsNullOrEmpty(_npcId))
                throw new NullReferenceException($"Quest Giver name cant be null");
        }

        [Command("Quest-giver", MonoTargetType.All)]
        public void CheckQuests()
        {
            var x = RefreshQuests();

            if (x.Count <= 0)
                ConsoleLogger.Log(this, $"{_npcId}: 0 / {_quests.Count} quests are available now");
            else
            {
                foreach (var item in _quests)
                {
                    ConsoleLogger.Log(this, $"{_npcId}: {item.ID} is available for pickup");
                }
            }
        }


        public IReadOnlyList<Quest> RefreshQuests()
        {
            List<Quest> quests = new();

            foreach (QuestSO so in _quests)
            {
                Quest quest = _questManager.GetQuestByID(so.ID);
                if (quest == null) continue;

                _questManager.CheckQuestPrerequisites(quest);
                quests.Add(quest);
            }

            return quests;
        }
    }
}

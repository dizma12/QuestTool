using QFSW.QC;
using QuestMaker.Domain;
using QuestMaker.Domain.Helpers;
using QuestMaker.Domain.Interactions;
using QuestMaker.Domain.Quests;
using QuestMaker.Runtime.Quests;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    public class QuestGiver : MonoBehaviour, IInteractable
    {
        [SerializeField, ReadOnlyInspector] private string _id = string.Empty;
        public string ID => _id;

        private QuestManager _questManager;

        private void Start()
        {
            if (string.IsNullOrEmpty(_id))
                throw new ArgumentException("QuestGiver has no id");

            _questManager = ReferenceManager.Instance.GetReference<QuestManager>();

            if (_questManager == null)
                ConsoleLogger.LogError(this, "QuestManager reference not found");
        }

        [Command("Quest-giver", MonoTargetType.All)]
        public void CheckQuests()
        {
            IReadOnlyList<Quest> available = RefreshQuests();

            if (available.Count <= 0)
                ConsoleLogger.Log(this, $"{ID}: 0 quests available now");
            else
                foreach (Quest quest in available)
                    ConsoleLogger.Log(this, $"{ID}: {quest.ID} is available for pickup");
        }

        public IReadOnlyList<Quest> RefreshQuests()
        {
            IReadOnlyList<Quest> quests = _questManager.GetHandInQuests(_id);
            foreach (Quest quest in quests)
                _questManager.CheckQuestPrerequisites(quest);
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
            foreach (Quest quest in _questManager.GetTurnInQuests(_id))
            {
                if (quest.Status != QuestStatus.CAN_FINISH) continue;

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
            foreach (Quest quest in _questManager.GetHandInQuests(_id))
            {
                if (quest.Status != QuestStatus.CAN_START) continue;

                if (_questManager.TryStartQuest(quest.ID))
                {
                    ConsoleLogger.Log(this, $"{ID}: handed in {quest.ID}");
                    return true;
                }
            }
            return false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            string assetGuid = UnityEditor.AssetDatabase.AssetPathToGUID(UnityEditor.AssetDatabase.GetAssetPath(this));
            if (!string.IsNullOrEmpty(assetGuid) && !_id.Equals(assetGuid))
            {
                _id = assetGuid;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}

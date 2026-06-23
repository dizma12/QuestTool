using QFSW.QC;
using QuestMaker.Domain;
using QuestMaker.Domain.Helpers;
using QuestMaker.Domain.Interactions;
using QuestMaker.Domain.Quests;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Quests;
using QuestMaker.Runtime.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class QuestGiver : MonoBehaviour, IInteractable
    {
        [SerializeField, ReadOnlyInspector] private string _id = string.Empty;
        public string ID => _id;

        private QuestManager _questManager;

        private List<Quest> _handIns = null;
        private List<Quest> _turnIns = null;
        UIEventBus _uiEventBus = null;
        private void Start()
        {
            if (string.IsNullOrEmpty(_id))
                throw new ArgumentException("QuestGiver has no id!");

            _questManager = ReferenceManager.Instance.RequestReference<QuestManager>();

            if (_questManager == null)
            {
                ConsoleLogger.LogError(this, "QuestManager reference not found!");
                return;
            }
            _uiEventBus = ReferenceManager.Instance.RequestReference<GameEventManager>().RequestBus<UIEventBus>();

            if (_uiEventBus == null)
            {
                ConsoleLogger.LogError(this, "Failled to find reference of UIEventBus!");
                return;
            }

            UpdateQuestLists();
        }

        [Command("Quest-giver", MonoTargetType.All)]
        public void CheckQuests()
        {
            IReadOnlyList<Quest> available = RefreshHandInQuests();

            if (available.Count <= 0)
                ConsoleLogger.Log(this, $"{ID}: No quests available now");
            else
                foreach (Quest quest in available)
                    ConsoleLogger.Log(this, $"{ID}: {quest.ID} is available for pickup");
        }

        private IReadOnlyList<Quest> RefreshHandInQuests()
        {
            List<Quest> canStart = new();
            foreach (Quest quest in _handIns)
            {
                if (quest == null || quest.Status != QuestStatus.CAN_START) continue;

                if (_questManager.CheckQuestPrerequisites(quest))
                    canStart.Add(quest);
            }
            return canStart;
        }
        private IReadOnlyList<Quest> GetTurnInQuests()
            => _turnIns.Where(q => q != null && q.Status == QuestStatus.CAN_FINISH).ToList();


        public void Interact(GameObject other)
        {
            ConsoleLogger.Log(this, $"{other.name} interacted with with quest giver: {ID}!");

            TryOpenQuestWindow();
        }

        private bool TryOpenQuestWindow()
        {

            IReadOnlyList<Quest> toStart = RefreshHandInQuests();
            IReadOnlyList<Quest> toFinish = GetTurnInQuests();

            if (toStart.Count <= 0 && toFinish.Count <= 0) return false;

            _uiEventBus.FireShowQuestGiverWindow(toStart, toFinish);
            return true;

        }
        private void UpdateQuestLists()
        {
            _handIns = _questManager.GetHandInQuests(_id).ToList();
            _turnIns = _questManager.GetTurnInQuests(_id).ToList();
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            _uiEventBus.FireCloseQuestGiverWindow();
        }
#if UNITY_EDITOR
        //When saving a quest giver as a prefab we need to save the GUID as the ID.
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

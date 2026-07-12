using QuestMaker.Domain;
using QuestMaker.Domain.Helpers;
using QuestMaker.Domain.Interactions;
using QuestMaker.Domain.Quests;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Quests;
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

        protected QuestManager _questManager;

        protected HashSet<Quest> _questsHandIns = new();
        protected HashSet<Quest> _questsTurnIns = new();

        protected IEnumerable<Quest> AvailableHandIns => _questsHandIns.Where(q => q != null && q.Status == QuestStatus.CAN_START);
        protected IEnumerable<Quest> AvailableTurnIns => _questsTurnIns.Where(q => q != null && q.Status == QuestStatus.CAN_FINISH);
        protected IEnumerable<Quest> MissingReq => _questsHandIns.Where(q => q != null && q.Status == QuestStatus.MISSING_REQUIRMENTS);


        protected QuestGiverIndicators _indicators = null;

        private UIEventBus _uiEventBus = null;
        private QuestEventBus _questEventBus = null;

        #region UNITY_CALLBACKS

        private void Start() 
        {
            if (string.IsNullOrEmpty(_id))
                throw new ArgumentException("QuestGiver has no id!");

            ReferenceManager refmngr = ReferenceManager.Instance;
            GameEventManager gamemngr = refmngr.RequestReference<GameEventManager>();

            _questManager = refmngr.RequestReference<QuestManager>();

            if (_questManager == null)
            {
                ConsoleLogger.LogError(this, "QuestManager reference not found!");
                return;
            }

            //UI events
            _uiEventBus = gamemngr.RequestBus<UIEventBus>();
            if (_uiEventBus == null)
            {
                ConsoleLogger.LogError(this, "Failled to find reference of UIEventBus!");
                return;
            }

            //Quest Events
            _questEventBus = gamemngr.RequestBus<QuestEventBus>();
            if (_questEventBus == null)
            {
                ConsoleLogger.LogError(this, "Failled to find reference of QuestEventBus!");
                return;
            }
            _questEventBus.OnPrerequisitesChanged += UpdateIndicators;
            _questEventBus.OnNewQuestAdded += InitializeGiversQuests;

            //Indicators
            _indicators = GetComponentInChildren<QuestGiverIndicators>();
            if (_indicators != null)
            {
                _questEventBus.OnQuestStarted += HandleQuestChange;
                _questEventBus.OnQuestCanFinish += HandleQuestChange;
                _questEventBus.OnQuestCompleted += HandleQuestChange;
            }


            InitializeGiversQuests();

        }
        private void OnDisable()
        {
            if (_questEventBus != null)
            {
                _questEventBus.OnPrerequisitesChanged -= UpdateIndicators;
                _questEventBus.OnNewQuestAdded += InitializeGiversQuests;
            }

            if (_indicators != null)
            {
                _questEventBus.OnQuestStarted -= HandleQuestChange;
                _questEventBus.OnQuestCanFinish -= HandleQuestChange;
                _questEventBus.OnQuestCompleted -= HandleQuestChange;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _uiEventBus.FireCloseQuestGiverWindow();
            UpdateIndicators();
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
        #endregion

        private void HandleQuestChange(Quest quest)
        {
            UpdateIndicators();
        }

        public void Interact(GameObject other)
        {
            ConsoleLogger.Log(this, $"{other.name} interacted with with quest giver: {ID}!");

            TryOpenQuestWindow();
        }

        private bool TryOpenQuestWindow()
        {
            List<Quest> toStast = AvailableHandIns.ToList();
            List<Quest> toFinish = AvailableTurnIns.ToList();
            List<Quest> missing = MissingReq.ToList();

            if (toStast.Count <= 0 && toFinish.Count <= 0 && missing.Count <= 0) return false;

            _uiEventBus.FireShowQuestGiverWindow(toStast, toFinish, missing);
            return true;

        }

        /// <summary>
        /// Updates Quest Indicators based on Saved quest statuses.
        /// </summary>
        private void UpdateIndicators()
        {
            if (_indicators == null) return;

            if (AvailableHandIns.Any())
                _indicators.ActivateHandInIndicator();

            else if (AvailableTurnIns.Any())
                _indicators.ActivateTurnInIndicator();

            else if (_questsTurnIns.Any(q => q.Status == QuestStatus.IN_PROGRESS))
                _indicators.ActivateInProgressTurnInIndicator();

            else if (_questsHandIns.Any(q => q.Status == QuestStatus.MISSING_REQUIRMENTS))
                _indicators.ActivateMissingReqHandInIndicator();

            else
                _indicators.DeactivateIndicators();
        }


        /// <summary>
        /// Updates Quest Lists and Quest Indicators.
        /// </summary>
        private void InitializeGiversQuests()
        {
            _questsHandIns = _questManager.GetHandInQuests(ID).ToHashSet();
            _questsTurnIns = _questManager.GetTurnInQuests(ID).ToHashSet();

            UpdateIndicators();
        }
    }
}

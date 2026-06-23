using QuestMaker.Domain;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Quests;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestMaker.Runtime.UI
{
    internal class QuestGiverWindow : MonoBehaviour, IGameReference
    {
        [SerializeField] private Transform _panel = null;
        [SerializeField] private Transform _handInArea = null;
        [SerializeField] private Transform _turnInArea = null;

        [SerializeField] private GameObject _buttonPrefab = null;

        private List<GameObject> _spawned = null;
        private QuestManager _questManager = null;
        private UIEventBus _uiEventBus = null;
        private void Awake()
        {
            ReferenceManager mngr = ReferenceManager.Instance;
            if (mngr == null)
            {
                ConsoleLogger.LogError(this, "Failed to retrieve Reference Manager");
                return;
            }

            if (!mngr.SubScribeReference<QuestGiverWindow>(this))
                ConsoleLogger.LogError(this, "Failed to subscribe QuestGiverWindow");

            _spawned = new();

            _uiEventBus = mngr.RequestReference<GameEventManager>().RequestBus<UIEventBus>();

            if (_uiEventBus == null)
            {
                ConsoleLogger.LogError(this, "Failed to retrieve UI event Bus");
                return;
            }
            _uiEventBus.ShowQuestGiverWindow += Show;
            _uiEventBus.CloseQuestGiverWindow += Close;

            if(_panel.gameObject.activeInHierarchy)
                Close();
        }

        private void OnDisable()
        {
            if (ReferenceManager.Instance != null)
                ReferenceManager.Instance.UnsubscribeReference<QuestGiverWindow>();

            if (_uiEventBus != null)
            {
                _uiEventBus.ShowQuestGiverWindow -= Show;
                _uiEventBus.CloseQuestGiverWindow -= Close;
            }
        }

        public void Show(IReadOnlyList<Quest> handins, IReadOnlyList<Quest> turnIns)
        {
            if (_questManager == null)
                _questManager = ReferenceManager.Instance.RequestReference<QuestManager>();

            Clear();

            BuildQuestPanel(handins, turnIns);
            
            _panel.gameObject.SetActive(true);
        }
        private void BuildQuestPanel(IReadOnlyList<Quest> handins,  IReadOnlyList<Quest> turnIns)
        {
            if (handins != null && handins.Any())
            {
                foreach (Quest quest in handins)
                {
                    GameObject text = Instantiate(_buttonPrefab, _handInArea);
                    _spawned.Add(text);
                    text.name = quest.ID + " button";
                    text.GetComponentInChildren<TMP_Text>().text = quest.ID;

                    text.GetComponent<Button>().onClick.AddListener(() => Accept(quest.ID));
                }
            }

            if (turnIns != null && turnIns.Any())
            {
                foreach (Quest quest in turnIns)
                {
                    GameObject text = Instantiate(_buttonPrefab, _turnInArea);
                    _spawned.Add(text);
                    text.name = quest.ID + " button";
                    text.GetComponentInChildren<TMP_Text>().text = quest.ID;

                    text.GetComponent<Button>().onClick.AddListener(() => Finish(quest.ID));
                }
            }
        }
        public void Close()
        {
            Clear();
            _panel.gameObject.SetActive(false);
        }

        private void Accept(string questID)
        {
            _questManager.TryStartQuest(questID);
            Close();
        }
        private void Finish(string questID)
        {
            _questManager.TryTurnInQuest(questID);
            Close();
        }
        private void Clear()
        {
            foreach (GameObject text in _spawned)
                Destroy(text);

            _spawned.Clear();
        }
    }
}
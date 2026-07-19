using QuestMaker.Domain;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Quests;
using System;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

namespace QuestMaker.Runtime
{
    public class InventoryPanel : MonoBehaviour
    {
        private const int MAX_INVENTORY_SLOTS = 24;
        [SerializeField] private GameObject _inventoryPanel = null;
        [SerializeField] private GameObject _inventorySlotPrefab = null;
        [SerializeField] private InventorySlot[] _slots = new InventorySlot[MAX_INVENTORY_SLOTS];

        private Inventory _inventory = null;
        private int _activeSlots = 0;
        private bool _isPanelOpen = false;
        private GameEventBus _gameEventbus = null;
        private QuestEventBus _questEventBus = null;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (_inventoryPanel == null)
            {
                ConsoleLogger.LogError(this, "Inventory Panel cannot be null. Please assign it on InventoryPanel!");
                gameObject.SetActive(false);
                return;
            }

            if (_inventorySlotPrefab == null)
            {
                ConsoleLogger.LogError(this, "Inventory Slot prefab cannot be null. Please assign it on InventoryPanel!");
                gameObject.SetActive(false);
                return;
            }


            _inventory = ReferenceManager.Instance.RequestReference<InventoryManager>().Inventory;

            SubscribeToEvents();
            CloseInventoryPanel();
        }
        private void OnDisable()
        {
            UnsubscribeToEvents();
        }

        private void HandleQuestCompleted(Quest quest)
        {
            if(_isPanelOpen)
                BuildInventoryPanel();
        }

        private void HandleItemCollected(ItemStack stack)
        {
            if (_isPanelOpen)
                BuildInventoryPanel();
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.I) && !_isPanelOpen)
            {
                BuildInventoryPanel();
                _isPanelOpen = true;
            }
            else if (Input.GetKeyDown(KeyCode.I) && _isPanelOpen)
            {
                CloseInventoryPanel();
                _isPanelOpen = false;
            }
        }

        private void CloseInventoryPanel()
        {
            _inventoryPanel.SetActive(false);
        }

        private void InitializePanel()
        {
            _inventoryPanel.SetActive(false);
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == null)
                {
                    if (!Instantiate(_inventorySlotPrefab, transform).TryGetComponent(out InventorySlot slot))
                    {
                        ConsoleLogger.LogWarning(slot, "Failed to retrieve InventorySlot component");
                        continue;
                    }
                    slot.gameObject.SetActive(false);

                }
                else
                    _slots[i].gameObject.SetActive(false);
            }
        }
        private void BuildInventoryPanel()
        {
            _inventoryPanel.SetActive(true);
            ItemStack[] inv = _inventory.RetrieveAll();
            if (inv.Length == 0) return;

            int repeats = Mathf.Min(inv.Length, MAX_INVENTORY_SLOTS);

            for (int i = 0; i < repeats; i++)
            {
                _slots[i].PrepareInventorySlot(inv[i].Item.Display, inv[i].Amount);
                _slots[i].gameObject.SetActive(true);
            }

            if (repeats < _activeSlots)
            {
                for (int i = repeats; i < _activeSlots; i++)
                {
                    _slots[i].gameObject.SetActive(false);
                }
            }
            _activeSlots = repeats;

        }

        private void SubscribeToEvents()
        {
            if(ReferenceManager.Instance == null)  return;

            GameEventManager mngr = ReferenceManager.Instance.RequestReference<GameEventManager>();

            if( mngr == null) return;

            _gameEventbus = mngr.RequestBus<GameEventBus>();

            if (_gameEventbus == null)
            {
                ConsoleLogger.LogError(this, "Inventory Slot prefab cannot be null. Please assign it on InventoryPanel!");
                gameObject.SetActive(false);
                return;
            }
            else
            {
                _gameEventbus.OnItemCollected += HandleItemCollected;
            }



            _questEventBus = mngr.RequestBus<QuestEventBus>();
            if (_questEventBus == null)
            {
                ConsoleLogger.LogError(this, "Inventory Slot prefab cannot be null. Please assign it on InventoryPanel!");
                gameObject.SetActive(false);
                return;
            }
            else
            {
                _questEventBus.OnQuestCompleted += HandleQuestCompleted;

                InitializePanel();
            }
        }

        private void UnsubscribeToEvents()
        {
            if (ReferenceManager.Instance == null) return;

            GameEventManager mngr = ReferenceManager.Instance.RequestReference<GameEventManager>();

            if (mngr == null) return;

            mngr.RequestBus<GameEventBus>().OnItemCollected -= HandleItemCollected;
            mngr.RequestBus<QuestEventBus>().OnQuestCompleted -= HandleQuestCompleted;
        }
    }
}

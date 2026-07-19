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

        [SerializeField] private GameObject _inventoryPanel = null;
        [SerializeField] private GameObject _inventorySlotPrefab = null;
        [SerializeField] private InventorySlot[] _slots = new InventorySlot[RuntimeSettings.MAX_INVENTORY_CAPACITY];

        private Inventory _inventory = null;
        private int _activeSlots = 0;
        private bool _isPanelOpen = false;


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

            if(SubscribeToEvents())
                InitializePanel();
            else
                gameObject.SetActive(false);

            CloseInventoryPanel();
        }
        private void OnDisable()
        {
            UnsubscribeToEvents();
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

            int repeats = Mathf.Min(inv.Length, RuntimeSettings.MAX_INVENTORY_CAPACITY);

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

        private bool SubscribeToEvents()
        {
            if(ReferenceManager.Instance == null)  return false;

            GameEventManager mngr = ReferenceManager.Instance.RequestReference<GameEventManager>();

            if (mngr == null) return false;

            mngr.RequestBus<GameEventBus>().OnInventoryChanged += HandleInventoryChanged;
            return true;

        }

        private void HandleInventoryChanged()
        {
            if (_isPanelOpen)
                BuildInventoryPanel();
        }

        private void UnsubscribeToEvents()
        {
            if (ReferenceManager.Instance == null) return;

            GameEventManager mngr = ReferenceManager.Instance.RequestReference<GameEventManager>();

            if (mngr == null) return;
            
            mngr.RequestBus<GameEventBus>().OnInventoryChanged -= HandleInventoryChanged;
        }
    }
}

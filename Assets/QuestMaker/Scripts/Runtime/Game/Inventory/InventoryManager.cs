using QuestMaker.Domain;
using QuestMaker.Domain.Steps;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Quests;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    [DefaultExecutionOrder(-17)]
    public class InventoryManager : MonoBehaviour, IGameReference
    {
        public Inventory Inventory { get; } = new();

        private GameEventBus _gameEventBus = null;
        private QuestEventBus _questEventBus = null;

        private void Awake()
        {
            if (!SubscribeSelf())
                ConsoleLogger.LogError(this, "Failed to Subscribe self on ReferenceManager");
        }

        private void Start()
        {
            GameEventManager mng = ReferenceManager.Instance.RequestReference<GameEventManager>();
            if (mng == null)
            {
                ConsoleLogger.LogError(this, "Failed to find reference of type GameEventManager");
                return;
            }
            _gameEventBus = mng.RequestBus<GameEventBus>();

            _gameEventBus.OnItemCollected += HandleItemGained;
            _gameEventBus.OnItemCrafted += HandleItemGained;
            _gameEventBus.OnItemDelivered += HandleItemDelivered;

            _questEventBus = mng.RequestBus<QuestEventBus>();
            _questEventBus.OnQuestCompleted += HandleQuestCompleted;
        }

        private void OnDisable()
        {
            UnsubscribeSelf();

            if (_gameEventBus != null)
            {
                _gameEventBus.OnItemCollected -= HandleItemGained;
                _gameEventBus.OnItemCrafted -= HandleItemGained;
                _gameEventBus.OnItemDelivered -= HandleItemDelivered;
            }
            if (_questEventBus != null)
                _questEventBus.OnQuestCompleted -= HandleQuestCompleted;
        }

        private void HandleItemGained(ItemStack stack) => Inventory.AddItemStack(stack.Item, stack.Amount);

        private void HandleItemDelivered(ItemStack stack) => Inventory.RemoveItemStack(stack.Item, stack.Amount);

        private void HandleQuestCompleted(Quest quest)
        {
            foreach (QuestStep step in quest.AllSteps)
            {
                if (step is ItemQuestStep itemStep)
                {
                    if (itemStep.QuestItem.Item.IsExclusivelyQuestItem)
                        Inventory.RemoveItem(itemStep.QuestItem);
                    else
                        Inventory.RemoveItemStack(itemStep.QuestItem.Item, itemStep.QuestItem.Amount);
                }
            }
        }

        private bool SubscribeSelf()
            => ReferenceManager.Instance.SubScribeReference<InventoryManager>(this);

        private bool UnsubscribeSelf()
        {
            if (ReferenceManager.Instance != null)
                return ReferenceManager.Instance.UnsubscribeReference<InventoryManager>();

            return false;
        }
    }
}

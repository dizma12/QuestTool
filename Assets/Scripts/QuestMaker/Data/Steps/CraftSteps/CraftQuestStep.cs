
using QuestMaker.Domain.Events;
using System;
using UnityEngine;


namespace QuestMaker.Domain.Steps
{
    internal class CraftQuestStep : QuestStep
    {

        private ItemStack _craftable = default;
        private int _currentAmount = 0;

        public override string ProgressText => $"Crafted: {_craftable.Item.Name}  {_currentAmount}/{_craftable.Amount}";

        public override bool IsComplete { get; protected set; } = false;

        public CraftQuestStep(QuestStepData data, IQuestEventSource eventbus) : base(eventbus)
        {
            if (data == null)
            {
                ConsoleLogger.LogError(this, "QuestStepData is null");
                return;
            }
            if (data is not CraftStepData craftData)
            {
                ConsoleLogger.LogError(this, $"Expected CraftStepData, got {data?.GetType()}");
                return;
            }
            if (craftData.CraftableItem.Item == null || craftData.CraftableItem.Amount <= 0)
            {
                ConsoleLogger.LogError(this, $"Invalind ItemAmount pair with item: {craftData.CraftableItem.Item} and amount: {craftData.CraftableItem.Amount}");
                return;
            }

            _craftable = craftData.CraftableItem;
        }

        private void HandleItemCraft(string itemID)
        {
            if (!_craftable.Item.ID.Equals(itemID)) return;

            _currentAmount++;
            FireOnChanged();

            ConsoleLogger.Log(this, ProgressText);

            if (Validate())
                Finish();

        }

        protected override bool Validate() => _currentAmount >= _craftable.Amount;

        protected override void Subscribe() => _eventBus.OnItemCrafted += HandleItemCraft;
        protected override void Unsubscribe() => _eventBus.OnItemCrafted -= HandleItemCraft;
    }
}

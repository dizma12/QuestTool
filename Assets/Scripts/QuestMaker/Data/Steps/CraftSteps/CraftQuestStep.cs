using QuestMaker.Domain.Events;

namespace QuestMaker.Domain.Steps
{
    internal class CraftQuestStep : ItemQuestStep
    {
        public override string ProgressText => $"Crafted: {_questItem.Item.Name}  {_currentAmount}/{_questItem.Amount}";

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

            _questItem = craftData.CraftableItem;
        }

        protected override void Subscribe() => _eventBus.OnItemCrafted += HandleItemProgress;
        protected override void Unsubscribe() => _eventBus.OnItemCrafted -= HandleItemProgress;
    }
}

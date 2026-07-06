using QuestMaker.Domain.Events;

namespace QuestMaker.Domain.Steps
{
    internal class LootQuestStep : ItemQuestStep
    {
        public override string ProgressText => $"{_questItem.Item.Name} looted: {_currentAmount}/{_questItem.Amount}";

        public LootQuestStep(QuestStepData data, IQuestEventSource eventBus) : base(eventBus)
        {
            if (data == null)
            {
                ConsoleLogger.LogError(this, "QuestStepData is null");
                return;
            }
            if (data is not LootStepData lootData)
            {
                ConsoleLogger.LogError(this, $"Expected LootStepData, got {data?.GetType()}");
                return;
            }
            if (lootData.Loot.Item == null || lootData.Loot.Amount <= 0)
            {
                ConsoleLogger.LogError(this, $"Invalind ItemAmount pair with item: {lootData.Loot.Item} and amount: {lootData.Loot.Amount}");
                return;
            }

            _questItem = lootData.Loot;
        }

        protected override void Subscribe() => _eventBus.OnItemCollected += HandleItemProgress;
        protected override void Unsubscribe() => _eventBus.OnItemCollected -= HandleItemProgress;
    }
}

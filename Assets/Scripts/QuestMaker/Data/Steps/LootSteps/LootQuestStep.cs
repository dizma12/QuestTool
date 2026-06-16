using QuestMaker.Domain.Events;

namespace QuestMaker.Domain.Steps
{
    internal class LootQuestStep : QuestStep
    {
        private readonly ItemStack _lootable = default;

        private int _currentAmount = 0;

        public override string ProgressText => $"{_lootable.Item.Name} looted: {_currentAmount}/{_lootable.Amount}";

        public override bool IsComplete { get; protected set; } = false;

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

            _lootable = lootData.Loot;
        }

        private void HandleItemCollection(string itemID)
        {
            if (!_lootable.Item.ID.Equals(itemID))
                return;

            _currentAmount++;
            FireOnChanged();

            ConsoleLogger.Log(this, ProgressText);

            if (Validate())
                Finish();
        }
        protected override bool Validate() => _currentAmount >= _lootable.Amount;

        protected override void Subscribe() => _eventBus.OnItemCollected += HandleItemCollection;
        protected override void Unsubscribe() => _eventBus.OnItemCollected -= HandleItemCollection;
    }
}

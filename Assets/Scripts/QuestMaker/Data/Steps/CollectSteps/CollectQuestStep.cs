using QuestMaker.Domain.Events;

namespace QuestMaker.Domain.Steps
{
    internal class CollectQuestStep : QuestStep
    {
        private readonly ItemStack _collectable = default;

        private int _currentAmount = 0;

        public override string ProgressText => $"{_collectable.Item.Name} collected: {_currentAmount}/{_collectable.Amount}";

        public override bool IsComplete { get; protected set; } = false;

        public CollectQuestStep(QuestStepData data, IQuestEventSource eventBus) : base(eventBus)
        {
            if (data == null)
            {
                ConsoleLogger.LogError(this, "QuestStepData is null");
                return;
            }
            if (data is not CollectStepData collectData)
            {
                ConsoleLogger.LogError(this, $"Expected CollectStepData, got {data?.GetType()}");
                return;
            }
            if (collectData.CollectableItem.Item == null || collectData.CollectableItem.Amount <= 0)
            {
                ConsoleLogger.LogError(this, $"Invalind ItemAmount pair with item: {collectData.CollectableItem.Item} and amount: {collectData.CollectableItem.Amount}");
                return;
            }

            _collectable = collectData.CollectableItem;
        }

        private void HandleItemCollection(string itemID)
        {
            ConsoleLogger.Log(this, $"Item Collected: {itemID}");
            ConsoleLogger.Log(this, $"Item Looking for: {_collectable.Item.ID}");
            if (!_collectable.Item.ID.Equals(itemID))
                return;

            _currentAmount++;
            FireOnChanged();

            //ConsoleLogger.Log(this, ProgressText);

            if (Validate())
                Finish();

        }
        protected override bool Validate() => _currentAmount >= _collectable.Amount;

        protected override void Subscribe() => _eventBus.OnItemCollected += HandleItemCollection;
        protected override void Unsubscribe() => _eventBus.OnItemCollected -= HandleItemCollection;
    }
}

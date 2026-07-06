using QuestMaker.Domain.Events;

namespace QuestMaker.Domain.Steps
{
    internal class CollectQuestStep : ItemQuestStep
    {
        public override string ProgressText => $"{_questItem.Item.Name} collected: {_currentAmount}/{_questItem.Amount}";

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

            _questItem = collectData.CollectableItem;
        }

        protected override void Subscribe() => _eventBus.OnItemCollected += HandleItemProgress;
        protected override void Unsubscribe() => _eventBus.OnItemCollected -= HandleItemProgress;
    }
}

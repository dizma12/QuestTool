

using QuestMaker.Domain.Events;

namespace QuestMaker.Domain.Steps
{
    public class DeliverQuestStep : QuestStep
    {
        private readonly Item _deliverable = null;
        private readonly string _npcID = string.Empty;

        private bool _isDelivered = false;

        public override string ProgressText => $"Deeliver {_deliverable.Name} to {_npcID}";

        public override bool IsComplete { get; protected set; } = false;

        public DeliverQuestStep(QuestStepData data, IQuestEventSource eventBus) : base(eventBus)
        {
            if (data == null)
            {
                ConsoleLogger.LogError(this, "QuestStepData is null");
                return;
            }
            if (data is not DeliverStepData deliverData)
            {
                ConsoleLogger.LogError(this, $"Expected DeliverStepData, got {data?.GetType()}");
                return;
            }
            if (deliverData.Item == null || string.IsNullOrEmpty(deliverData.NpcID))
            {
                ConsoleLogger.LogError(this, $"Invalind Deliver pair with item: {deliverData.Item} and amount: {deliverData.NpcID}");
                return;
            }

            _deliverable = deliverData.Item;
            _npcID = deliverData.NpcID;
        }

        private void HandleDelivery(string itemID)
        {
            if (_isDelivered)
            {
                Finish();
                return;
            }

            if (_deliverable.ID.Equals(itemID) && !_isDelivered)
            {
                _isDelivered = true;
                FireOnChanged();
                ConsoleLogger.LogWarning(this, $"Finished is not automaticly called for deliver step!");
            }
        }
        protected override bool Validate() => _isDelivered;

        protected override void Subscribe() => _eventBus.OnItemDelivered += HandleDelivery;
        protected override void Unsubscribe() => _eventBus.OnItemDelivered -= HandleDelivery;
    }
}

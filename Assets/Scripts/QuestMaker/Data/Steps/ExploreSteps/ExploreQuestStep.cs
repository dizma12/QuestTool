using QuestMaker.Domain.Events;



namespace QuestMaker.Domain.Steps
{
    internal class ExploreQuestStep : QuestStep
    {
        private readonly string _areaID = string.Empty;
        private bool _isExplored = false;

        public override string ProgressText => $"Explore {_areaID}";

        public override bool IsComplete { get; protected set; } = false;

        public ExploreQuestStep(QuestStepData data, IQuestEventSource eventbus) : base(eventbus)
        {
            if (data == null)
            {
                ConsoleLogger.LogError(this, "QuestStepData is null");
                return;
            }
            if (data is not ExploreStepData exploreData)
            {
                ConsoleLogger.LogError(this, $"Expected ExploreStepData, got {data?.GetType()}");
                return;
            }
            if (string.IsNullOrEmpty(exploreData.AreaID))
            {
                ConsoleLogger.LogError(this, $"Invalind Aread ID");
                return;
            }
            _areaID = exploreData.AreaID;
        }


        private void HandleAreaEntered(string areaID)
        {
            if (_isExplored)
            {
                Finish();
                return;
            }

            if (_areaID.Equals(areaID) && !_isExplored)
            {
                _isExplored = true;
                FireOnChanged();
                ConsoleLogger.LogWarning(this, $"Finished is not automaticly called for explore step!");
            }
        }

        protected override bool Validate() => _isExplored;

        protected override void Subscribe() => _eventBus.OnAreaEntered += HandleAreaEntered;
        protected override void Unsubscribe() => _eventBus.OnAreaEntered -= HandleAreaEntered;
    }
}

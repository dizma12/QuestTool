
using QuestMaker.Domain.Events;

namespace QuestMaker.Domain.Steps
{
    public class SlayQuestStep : QuestStep
    {
        private readonly string _enemyID = string.Empty;
        private readonly int _requiredAmount = 1;
        private int _currentKillCount = 0;

        public override string ProgressText => $"{_enemyID} slained {_currentKillCount}/{_requiredAmount}";

        public override bool IsComplete { get; protected set; } = false;

        public SlayQuestStep(QuestStepData data, IQuestEventSource eventbus) : base(eventbus)
        {
            if (data == null)
            {
                ConsoleLogger.LogError(this, "QuestStepData is null");
                return;
            }
            if (data is not SlayStepData slayData)
            {
                ConsoleLogger.LogError(this, $"Expected SlayStepData, got {data?.GetType()}");
                return;
            }
            if (string.IsNullOrEmpty(slayData.EnemyID) || slayData.Amount <= 0)
            {
                ConsoleLogger.LogError(this, $"Invalind SlayData with EnemyID: {slayData.EnemyID} and amount: {slayData.Amount}");
                return;
            }

            _enemyID = slayData.EnemyID;
            _requiredAmount = slayData.Amount;
        }

        private void HandleEnemyKilled(string killedEnemyID)
        {
            if (!_enemyID.Equals(killedEnemyID)) return;

            _currentKillCount++;
            FireOnChanged();
            ConsoleLogger.Log(this, ProgressText);

            if (Validate())
                Finish();
        }

        protected override bool Validate() => _currentKillCount >= _requiredAmount;

        protected override void Subscribe() => _eventBus.OnEnemyKilled += HandleEnemyKilled;
        protected override void Unsubscribe() => _eventBus.OnEnemyKilled -= HandleEnemyKilled;
    }
}

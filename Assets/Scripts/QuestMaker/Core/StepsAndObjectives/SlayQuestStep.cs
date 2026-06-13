
using QuestMaker.Data.Steps;
using QuestMaker.Runtime.Events.Handlers;
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Game.Events;
using UnityEngine;

namespace QuestMaker.Runtime.StepsAndObjectives
{
    public class SlayQuestStep : QuestStep
    {
        private string _enemyID = string.Empty;
        private int _requiredAmount = 1;
        private int _currentKillCount = 0;
        private GameEventHandler _eventHandler = null;
 
        public SlayQuestStep(QuestStepData data) : base(data) { }

        public override void Initialize(QuestStepData data)
        {
            if (data == null)
            {
                Debug.LogError($"[SlayQuestStep] QuestStepData is null");
                return;
            }
            if (data is not SlayStepData slayData)
            {
                Debug.LogError($"[SlayQuestStep] Expected SlayStepData, got {data?.GetType()}");
                return;
            }

            _enemyID = slayData.EnemyID;
            _requiredAmount = slayData.Amount;
            _currentKillCount = 0;

            // Subscribe to the kill event once initialized
            _eventHandler = ReferenceManager.Instance
                .GetReference<GameEventManager>()
                .GetEventHandler<GameEventHandler>();

            _eventHandler.OnEnemyKilled += HandleEnemyKilled;
        }

        private void HandleEnemyKilled(string killedEnemyID)
        {
            if (!_enemyID.Equals(killedEnemyID)) return;

            _currentKillCount++;
            Debug.Log($"[SlayQuestStep] {_currentKillCount}/{_requiredAmount} {_enemyID} slain");

            if (Validate())
                FinishStep();
        }

        public override bool Validate() => _currentKillCount >= _requiredAmount;

        protected override void FinishStep()
        {

            _eventHandler.OnEnemyKilled -= HandleEnemyKilled;

            base.FinishStep();
        }

        private void OnDestroy()
        {
            if (_eventHandler != null)
                _eventHandler.OnEnemyKilled -= HandleEnemyKilled;
        }
    }
}
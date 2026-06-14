using QuestMaker.Domain.Events;
using UnityEngine;

namespace QuestMaker.Domain.Steps
{
    [System.Serializable]
    public class SlayStepData : QuestStepData
    {
        public string StepID => stepId;

        [SerializeField, HideInInspector] private string stepId = string.Empty;

        public string EnemyID
        {
            get => enemyID;
            set
            {
                if (string.IsNullOrEmpty(enemyID))
                {
                    enemyID = value;
                    SetStepID();
                }
            }
        }
        public int Amount
        {
            get => amount;
            set
            {
                if (amount <= 0)
                {
                    amount = value;
                    SetStepID();
                }
            }
        }

        [SerializeField] private string enemyID = string.Empty;
        [SerializeField] private int amount = 0;
        private void SetStepID()
        {
            stepId = $"Slay_[{enemyID}]_{Amount}";
        }
        public override IRuntimeStep CreateRuntimeStep(IQuestEventSource eventbus) => new SlayQuestStep(this, eventbus);

    }
}

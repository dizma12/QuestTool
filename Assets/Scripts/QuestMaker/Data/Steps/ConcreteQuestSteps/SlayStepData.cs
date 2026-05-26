using UnityEngine;

namespace QuestMaker.Data.Steps
{
    [System.Serializable]
    public class SlayStepData : QuestStepData
    {
        public override QuestStepType StepType => QuestStepType.Slay;
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
        private void SetStepID()
        {
            stepId = $"{StepType}_[{enemyID}]_{Amount}";
        }

        [SerializeField] private string enemyID = string.Empty;
        [SerializeField] private int amount = 0;
    }
}
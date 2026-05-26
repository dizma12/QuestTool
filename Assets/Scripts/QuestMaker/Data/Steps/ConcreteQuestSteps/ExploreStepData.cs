using UnityEngine;

namespace QuestMaker.Data.Steps
{
    [System.Serializable]
    public class ExploreStepData : QuestStepData
    {
        public override QuestStepType StepType => QuestStepType.Explore;
        public string StepID => stepId;


        [SerializeField, HideInInspector]
        private string stepId = string.Empty;
        public string AreaID
        {
            get => area;
            set
            {
                if (string.IsNullOrEmpty(area)
                    && !string.IsNullOrEmpty(value))
                {
                    area = value;
                    SetStepID();
                }
            }
        }
        [SerializeField] private string area = string.Empty;

        private void SetStepID()
        {
            stepId = $"{StepType}_[{AreaID}]";
        }
    }
}

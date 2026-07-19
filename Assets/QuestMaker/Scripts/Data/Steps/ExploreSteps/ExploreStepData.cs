using QuestMaker.Domain.Events;
using UnityEngine;

namespace QuestMaker.Domain.Steps
{
    [System.Serializable]
    public class ExploreStepData : QuestStepData
    {
        
        public string StepID => stepId;


        [SerializeField, HideInInspector]
        private string stepId = string.Empty;

        [SerializeField] private string area = string.Empty;
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
        public override IRuntimeStep CreateRuntimeStep(IQuestEventSource eventbus) => new ExploreQuestStep(this, eventbus);
        private void SetStepID()
        {
            stepId = $"Explore_[{AreaID}]";
        }
    }
}

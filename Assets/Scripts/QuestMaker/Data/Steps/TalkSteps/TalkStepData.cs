using QuestMaker.Domain.Events;
using UnityEngine;

namespace QuestMaker.Domain.Steps
{
    [System.Serializable]
    public class TalkStepData : QuestStepData
    {
        public override IRuntimeStep CreateRuntimeStep(IQuestEventSource eventbus) => new TalkQuestStep(this, eventbus);
        public string StepID => stepId;


        [SerializeField, HideInInspector]
        private string stepId = string.Empty;

        [SerializeField] private string npc = string.Empty;
        public string NpcID
        {
            get => npc;
            set
            {
                if (string.IsNullOrEmpty(npc)
                    && !string.IsNullOrEmpty(value))
                {
                    npc = value;
                    SetStepID();
                }
            }
        }


        private void SetStepID()
        {
            stepId = $"Talk_[{npc}]";
        }

    }
}

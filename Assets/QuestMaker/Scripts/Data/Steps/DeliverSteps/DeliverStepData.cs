using QuestMaker.Domain.Events;
using UnityEngine;

namespace QuestMaker.Domain.Steps
{
    [System.Serializable]
    public class DeliverStepData : QuestStepData
    {

        public string StepID => stepId;

        [SerializeField, HideInInspector]
        private string stepId = string.Empty;

        [SerializeField] private Item item;

        [SerializeField] private string npc = string.Empty;
        public Item Item
        {
            get => item;
            set
            {
                if (item == null && value != null)
                {
                    item = value;
                    SetStepID();
                }
            }
        }

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
        public override IRuntimeStep CreateRuntimeStep(IQuestEventSource eventbus) => new DeliverQuestStep(this, eventbus);
        private void SetStepID()
        {
            stepId = $"Deliver_[{item.Name}]_{npc}";
        }
    }
}

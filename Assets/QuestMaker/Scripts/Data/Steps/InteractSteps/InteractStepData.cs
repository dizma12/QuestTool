using QuestMaker.Domain.Events;
using UnityEngine;

namespace QuestMaker.Domain.Steps
{
    [System.Serializable]
    public class InteractStepData : QuestStepData
    {

        public string StepID => stepId;


        [SerializeField, HideInInspector]
        private string stepId = string.Empty;
        public IInteractable Item
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

        [SerializeField] private IInteractable item;
        private void SetStepID()
        {
            stepId = $"Interact_[{item}]";
        }

        public override IRuntimeStep CreateRuntimeStep(IQuestEventSource eventbus) => new InteractQuestStep(this, eventbus);

        public interface IInteractable
        {
            void OnInteracted();
        }

    }
}

using UnityEngine;

namespace QuestMaker.Data.Steps
{
    [System.Serializable]
    public class InteractStepData : QuestStepData
    {
        public override QuestStepType StepType => QuestStepType.Interact;

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
            stepId = $"{StepType}_[{item}]";
        }
    }

    public interface IInteractable
    {
        void OnInteracted();
    }


}

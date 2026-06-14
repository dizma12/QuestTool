using QuestMaker.Domain.Events;
using UnityEngine;

namespace QuestMaker.Domain.Steps
{
    [System.Serializable]
    public class CraftStepData : QuestStepData
    {


        [SerializeField, HideInInspector]
        private string _stepId = string.Empty;
        public string StepID => _stepId;

        [SerializeField] private ItemAmount _craftable;
        public ItemAmount CraftableItem
        {
            get => _craftable;
            set
            {
                if (value.Item != null && value.Amount > 0)
                {
                    _craftable = value;
                    SetStepID();
                }
            }
        }
        public override IRuntimeStep CreateRuntimeStep(IQuestEventSource eventbus) => new CraftQuestStep(this, eventbus);

        private void SetStepID()
        {
            _stepId = $"Craft_[{_craftable.Item.Name}]_{_craftable.Amount}";
        }
    }
}

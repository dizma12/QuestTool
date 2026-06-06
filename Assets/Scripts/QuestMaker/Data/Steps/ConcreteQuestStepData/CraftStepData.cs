using UnityEngine;

namespace QuestMaker.Data.Steps
{
    [System.Serializable]
    public class CraftStepData : QuestStepData
    {
        public override StepCategory StepType => StepCategory.Craft;


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

        private void SetStepID()
        {
            _stepId = $"{StepType}_[{_craftable.Item.Name}]_{_craftable.Amount}";
        }
    }
}

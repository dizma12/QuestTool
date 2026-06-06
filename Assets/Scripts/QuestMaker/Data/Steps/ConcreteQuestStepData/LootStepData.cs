using QuestMaker.Data.Steps;

using UnityEngine;

namespace QuestMaker.Data.Steps
{
    [System.Serializable]
    public class LootStepData : QuestStepData
    {
        public override StepCategory StepType => StepCategory.Loot;
        public string StepID => stepId;

        [SerializeField]
        private ItemAmount _item;

        [SerializeField, HideInInspector]
        private string stepId = string.Empty;

        public ItemAmount Loot
        {
            get => _item;
            set
            {
                if (!_item.Equals(default(ItemAmount)))
                    return;

                if (value.Item != null && value.Amount > 0)
                {
                    _item = value;
                    SetStepID();
                }
            }
        }

        private void SetStepID()
        {
            stepId = $"{StepType}_[{_item.Item.Name}]_{_item.Amount}";
        }
    }
}
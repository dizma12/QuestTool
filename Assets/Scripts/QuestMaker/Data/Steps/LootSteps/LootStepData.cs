using QuestMaker.Domain.Events;
using UnityEngine;

namespace QuestMaker.Domain.Steps
{
    [System.Serializable]
    public class LootStepData : QuestStepData
    {
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
        public override IRuntimeStep CreateRuntimeStep(IQuestEventSource eventbus) => new LootQuestStep(this, eventbus);

        private void SetStepID()
        {
            stepId = $"Loot_[{_item.Item.Name}]_{_item.Amount}";
        }
    }
}

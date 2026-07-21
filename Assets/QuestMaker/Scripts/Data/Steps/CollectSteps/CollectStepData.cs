using QuestMaker.Domain.Events;
using UnityEngine;

namespace QuestMaker.Domain.Steps
{
    [System.Serializable]
    public class CollectStepData : QuestStepData
    {
        public string StepID => _stepId;

        [SerializeField, HideInInspector]
        private string _stepId = string.Empty;

        [SerializeField] private ItemStack _collectable;
        public ItemStack CollectableItem
        {
            get => _collectable;
            set
            {
                if (value.Item != null && value.Amount > 0)
                {
                    _collectable = value;
                    SetStepID();
                }
            }
        }

        public override IRuntimeStep CreateRuntimeStep(IQuestEventSource eventbus) => new CollectQuestStep(this, eventbus);
        private void SetStepID()
        {
            _stepId = $"Collect_[{_collectable.Item.Name}]_{_collectable.Amount}";
        }
    }
}

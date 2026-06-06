
using UnityEngine;

namespace QuestMaker.Data.Steps
{
    [System.Serializable]
    public class CollectStepData : QuestStepData
    {
        public override StepCategory StepType => StepCategory.Collect;
        public string StepID => _stepId;

        [SerializeField, HideInInspector]
        private string _stepId = string.Empty;

        [SerializeField] private ItemAmount _collectable;
        public ItemAmount CollectableItem
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

        private void SetStepID()
        {
            _stepId = $"{StepType}_[{_collectable.Item.Name}]_{_collectable.Amount}";
        }
    }
}

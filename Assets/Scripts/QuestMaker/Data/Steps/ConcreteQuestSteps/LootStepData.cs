using QuestMaker.Data.Steps;

using UnityEngine;

namespace QuestMaker.Data.Steps
{
    [System.Serializable]
    public class LootStepData : QuestStepData
    {
        public override StepCategory StepType => StepCategory.Loot;
        public string StepID => stepId;
  

        [SerializeField, HideInInspector] 
        private string stepId = string.Empty;

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

        public int Amount
        {
            get => amount;
            set
            {
                if (amount <= 0 && value > 0)
                {
                    amount = value;
                    SetStepID();
                }
            }
        }

        [SerializeField] private Item item;
        [SerializeField] private int amount = 0;

        private void SetStepID()
        {
            stepId = $"{StepType}_[{item.ItemName}]_{amount}";
        }
    }
}
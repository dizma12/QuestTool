using UnityEngine;

namespace QuestMaker.Data.Steps
{
    [System.Serializable]
    public class CraftStepData : QuestStepData
    {
        public override StepCategory StepType => StepCategory.Craft;


        [SerializeField, HideInInspector]
        private string stepId = string.Empty;
        public string StepID => stepId;

        [SerializeField] private Item item;
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

        [SerializeField] private int amount = 0;
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

        private void SetStepID()
        {
            stepId = $"{StepType}_[{item.ItemName}]_{amount}";
        }
    }
}

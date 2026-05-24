using QuestMaker.Data.Steps;
using System;
using UnityEngine;

namespace QuestMaker.Data
{
    [Serializable]
    public class LootStepData : QuestStepData
    {

        public override QuestStepType StepType => QuestStepType.Loot;
        public string StepID => stepId;

        [SerializeField, HideInInspector] 
        private string stepId = string.Empty;

        public Item Item
        {
            get => item;
            set
            {
                if (item == null)
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
                if (amount <= 0)
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
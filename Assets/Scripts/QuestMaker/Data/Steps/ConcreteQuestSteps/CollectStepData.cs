using QuestMaker.Data.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QuestMaker.Data.Steps
{
    internal class CollectStepData : QuestStepData
    {
        public override QuestStepType StepType => QuestStepType.Collect;

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
                if (amount <= 0 && value >= 1)
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

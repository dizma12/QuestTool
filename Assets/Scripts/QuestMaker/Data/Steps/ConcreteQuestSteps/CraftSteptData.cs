
using System;
using UnityEngine;

namespace QuestMaker.Data.Steps
{
    [Serializable]
    public class CraftSteptData : QuestStepData
    {
        public override QuestStepType StepType => QuestStepType.Craft;


        public Item Item
        {
            get => item;
            set
            {
                if (item == null)
                    item = value;
            }
        }

        public int Amount
        {
            get => amount;
            set
            {
                if (amount <= 0)
                    amount = value;
            }
        }

        [SerializeField] private Item item;
        [SerializeField] private int amount = 0;
    }
}

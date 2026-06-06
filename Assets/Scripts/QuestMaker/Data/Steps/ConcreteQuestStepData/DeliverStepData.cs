using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QuestMaker.Data.Steps
{
    public class DeliverStepData : QuestStepData
    {
        public override StepCategory StepType => StepCategory.Deliver;
        public string StepID => stepId;

        [SerializeField, HideInInspector]
        private string stepId = string.Empty;

        [SerializeField] private Item item;

        [SerializeField] private string npc = string.Empty;
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

        public string NpcID
        {
            get => npc;
            set
            {
                if (string.IsNullOrEmpty(npc)
                    && !string.IsNullOrEmpty(value))
                {
                    npc = value;
                    SetStepID();
                }
            }
        }
        private void SetStepID()
        {
            stepId = $"{StepType}_[{item.Name}]_{npc}";
        }
    }
}

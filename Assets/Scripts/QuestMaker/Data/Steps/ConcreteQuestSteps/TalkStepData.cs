using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QuestMaker.Data.Steps
{
    public class TalkStepData : QuestStepData
    {
        public override QuestStepType StepType => QuestStepType.Talk;
        public string StepID => stepId;


        [SerializeField, HideInInspector]
        private string stepId = string.Empty;

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
        [SerializeField] private string npc = string.Empty;


        private void SetStepID()
        {
            stepId = $"{StepType}_[{npc}]";
        }

    }
}

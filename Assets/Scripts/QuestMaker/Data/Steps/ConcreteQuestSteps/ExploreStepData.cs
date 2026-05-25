using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QuestMaker.Data.Steps
{
    public class ExploreStepData : QuestStepData
    {

        public override QuestStepType StepType => QuestStepType.Explore;
        public string AreaID
        {
            get => area;
            set
            {
                if (string.IsNullOrEmpty(area) 
                    && !string.IsNullOrEmpty(value))
                    area = value;
            }
        }
        [SerializeField] private string area = string.Empty;
    }
}

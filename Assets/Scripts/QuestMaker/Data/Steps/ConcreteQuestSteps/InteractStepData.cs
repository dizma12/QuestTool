using QuestMaker.Data.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QuestMaker.Data.Steps
{
    public class InteractStepData : QuestStepData
    {
        public override QuestStepType StepType => QuestStepType.Craft;


        public IInteractable Item
        {
            get => item;
            set
            {
                if (item == null && value != null)
                    item = value;
            }
        }

        [SerializeField] private IInteractable item;
    }

    public interface IInteractable
    {
        void OnInteracted();
    }
}

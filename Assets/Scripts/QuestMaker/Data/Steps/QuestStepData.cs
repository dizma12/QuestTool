using System;
using UnityEngine;
namespace QuestMaker.Data.Steps
{
    [Serializable]
    public abstract class QuestStepData
    {
        public abstract QuestStepType StepType { get; }

    }
}
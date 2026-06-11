using QuestMaker.Data.Steps;
using UnityEngine;


namespace QuestMaker.Runtime.StepsAndObjectives
{
    internal class InteractQuestStep : QuestStep
    {
        public InteractQuestStep(QuestStepData data) : base(data) { }


        public override void Initialize(QuestStepData data)
        {
            if (data == null)
            {
                Debug.LogError($"[InteractStepData] {data?.GetType()} is null");
                return;
            }
            if (data is not InteractStepData interactStep)
            {
                Debug.LogError($"[InteractStepData] Expected InteractStepData, got {data?.GetType()}");
                return;
            }
        }

        public override bool Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
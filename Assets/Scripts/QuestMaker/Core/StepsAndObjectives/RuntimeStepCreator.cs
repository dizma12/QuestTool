using QuestMaker.Data.Steps;
using QuestMaker.Runtime.StepsAndObjectives;
using System;


namespace QuestMaker.Core
{
    internal static class RuntimeStepCreator
    {
        //There are better ways to do this but who cares :/
        public static QuestStep CreateQuestStep(QuestStepData stepData)
        {

            return stepData.StepType switch
            {
                StepCategory.Slay => new SlayQuestStep(stepData),
                StepCategory.Loot => new LootQuestStep(stepData),
                StepCategory.Craft => new CraftQuestStep(stepData),
                StepCategory.Interact => new InteractQuestStep(stepData), // not working properly
                //StepCategory.Explore => new ExploreQuestStep(stepData),
                //StepCategory.Collect => new CollectQuestStep(stepData),
                //StepCategory.Talk => new TalkQuestStep(stepData),
                //StepCategory.Deliver => new DeliverQuestStep(stepData),
                _ => throw new ArgumentException("Invalid StepCategory"),
            };
        }
    }
}

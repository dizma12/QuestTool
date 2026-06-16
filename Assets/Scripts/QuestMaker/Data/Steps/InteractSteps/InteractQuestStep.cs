using QuestMaker.Domain.Events;

namespace QuestMaker.Domain.Steps
{
    internal class InteractQuestStep : QuestStep
    {
        public InteractQuestStep(QuestStepData data, IQuestEventSource eventbus) : base(eventbus) { throw new System.NotImplementedException(); }

        public override string ProgressText => throw new System.NotImplementedException();

        public override bool IsComplete { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }

        protected override bool Validate() => throw new System.NotImplementedException();

        protected override void Subscribe() => throw new System.NotImplementedException();
        protected override void Unsubscribe() => throw new System.NotImplementedException();
    }
}

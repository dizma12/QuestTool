using QuestMaker.Domain.Events;
using System;
namespace QuestMaker.Domain.Steps
{
    public abstract class QuestStep : IRuntimeStep
    {
        protected readonly IQuestEventSource _eventBus = null;

        public abstract string ProgressText { get; }

        public abstract bool IsComplete { get; protected set; }

        public event Action<QuestStep> Started; // runs when steps is started.
        public event Action<QuestStep> Finished; // runs when step is finished.
        public event Action<QuestStep> Changed; // from IRuntimeStep

        public QuestStep(IQuestEventSource eventbus)
        {
            if (eventbus == null)
            {
                ConsoleLogger.LogError(this, "Event bus cant be null");
                return;
            }
            _eventBus = eventbus;
        }

        protected abstract bool Validate();

        protected void FireOnStart() => Started?.Invoke(this);
        protected void FireOnChanged() => Changed?.Invoke(this);
        protected void FireOnFinished() => Finished?.Invoke(this);
            
        public void Start()
        {
            Subscribe();
            FireOnStart();
        }

        public void Finish()
        {
            IsComplete = true;
            Unsubscribe();
            FireOnFinished();
        }

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();
    }
}

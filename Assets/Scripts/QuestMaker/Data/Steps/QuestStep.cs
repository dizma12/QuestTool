using QuestMaker.Domain.Events;
using System;
namespace QuestMaker.Domain.Steps
{
    public abstract class QuestStep : IRuntimeStep
    {
        protected readonly IQuestEventSource _eventBus = null;

        public abstract string ProgressText { get; }

        public abstract bool IsComplete { get; }

        public event Action Started; // runs when steps is started.
        public event Action Finished; // runs when step is finished.
        public event Action Changed; // from IRuntimeStep

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

        protected void FireOnStart() => Started?.Invoke();
        protected void FireOnChanged() => Changed?.Invoke();
        protected void FireOnFinished() => Finished?.Invoke();

        public void Start()
        {
            Subscribe();
            FireOnStart();
        }

        public void Finish()
        {
            Unsubscribe();
            FireOnFinished();
        }

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();
    }
}

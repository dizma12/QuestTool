using System;

namespace QuestMaker.Domain.Steps
{
    public interface IRuntimeStep
    {
        public string ProgressText { get; }        // "Slay enemy X 5/9"
        public bool IsComplete { get; }
        public event Action<QuestStep> Changed;               // UI repaints on this
        public void Start();                    // subscribe to game events
        public void Finish();                  // unsubscribe
    }
}

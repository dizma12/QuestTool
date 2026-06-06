using QuestMaker.Data.Steps;
using QuestMaker.Runtime.Events.Handlers;
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Game.Events;
using UnityEngine;


namespace QuestMaker.Runtime.StepsAndObjectives
{
    public abstract class QuestStep
    {
        protected bool isFinished  = false;
        /// <summary>
        /// Fires Quest Step Finished on QuestEventHandler. Also sets is Finished to true.
        /// </summary>
        protected virtual void FinishStep()
        {
            if (isFinished) return;

            ReferenceManager.Instance.GetReference<GameEventManager>()
                                     .GetEventHandler<QuestEventHandler>()
                                     .FireOnQuestStepFinished(this);

            isFinished = true;
        }

        /// <summary>
        /// Constructor Calls Initialize() method.
        /// </summary>
        /// <param name="data"></param>
        public QuestStep(QuestStepData data)
        {
            Initialize(data);
        }
        public abstract void Initialize(QuestStepData data);
        public abstract bool Validate();
    }

}
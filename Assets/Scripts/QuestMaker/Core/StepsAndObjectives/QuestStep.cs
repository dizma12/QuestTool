using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Game.Events;
using UnityEngine;


namespace QuestMaker.Runtime.StepsAndObjectives
{
    public abstract class QuestStep : MonoBehaviour
    {
        private bool isFinished  = false;
        protected virtual void FinishStep()
        {
            if (isFinished) return;

            ReferenceManager.Instance.GetReference<GameEventManager>()
                                     .GetEventHandler<QuestEventHandler>()
                                     .QuestStepFinished(this);

            isFinished = true;
        }
    }

}
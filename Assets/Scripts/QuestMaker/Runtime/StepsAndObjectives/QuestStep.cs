using Assets.Scripts.QuestMaker.Game.Events;
using UnityEngine;


namespace QuestMaker.Runtime.StepsAndObjectives
{
    public abstract class QuestStep : MonoBehaviour
    {
        private bool isFinished  = false;

        protected void FinishStep()
        {
            if (isFinished) return;

            GlobalEventManager.Instance.QuestEvent.QuestFinished();

            isFinished = true;
        }
    }
}
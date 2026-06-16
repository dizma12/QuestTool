using QuestMaker.Runtime.Quests;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using QuestMaker.Runtime.Game;
using System;
using QuestMaker.Runtime.Events;

namespace QuestMaker.Runtime
{
    public class QuestTestDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text txt;
        [SerializeField] private TMP_Text txtStep;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            QuestEventBus qbus = ReferenceManager.Instance.GetReference<GameEventManager>().RequestBus<QuestEventBus>();

            qbus.OnQuestStarted += HandleQuestStart;
        }
        private void HandleQuestStart(Quest quest)
        {
            QuestManager qman = ReferenceManager.Instance.GetReference<QuestManager>();
            List<string> s = new();
            
            foreach(Quest q in qman.ActiveQuests)
            {
                q.Changed += HandleQuestStart;
                List<string> s2 = new();
                foreach(var c in q.CurrentSteps)
                {
                    s2.Add(c.ProgressText);
                }
                s.Add($"{q.ID}: {Environment.NewLine}{string.Join(Environment.NewLine, s2)}");
            }
            DisplayQuestText(txt, string.Join(Environment.NewLine, s));
        }

        private void DisplayQuestText(TMP_Text text, string msg)
        {
            text.text = msg;
        }
    }
}

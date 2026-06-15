using QuestMaker.Runtime.Quests;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using QuestMaker.Runtime.Game;
using QuestMaker.Domain.Objectives;
using QuestMaker.Domain.Steps;
using System;

namespace QuestMaker.Runtime
{
    public class QuestTestDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text txt;
        [SerializeField] private TMP_Text txtStep;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            var qmngr = ReferenceManager.Instance.GetReference<QuestManager>();
            List<string> objs = new();
            List<string> steps = new();
            foreach(Quest q in qmngr.QuestMap.Values)
            {
                foreach(ObjectiveData obj in q.Objectives)
                {
                    objs.Add($"{q.ID}: {obj.Description}");
                }
                
                foreach(QuestStep s in q.AllSteps)
                {
                    steps.Add($"{q.ID}: {s.ProgressText}");
                }
            }

            txt.text = string.Join(Environment.NewLine, objs);
            txtStep.text = string.Join(Environment.NewLine, steps);
        }

    }
}

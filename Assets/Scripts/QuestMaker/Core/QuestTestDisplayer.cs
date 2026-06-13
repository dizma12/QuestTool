using QuestMaker.Core.Quests;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
namespace QuestMaker.Core
{
    public class QuestTestDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text txt;
        [SerializeField] private TMP_Text txtStep;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //List<string> objectives = new();
            //List<string> steps = new();

            //foreach (var q in QuestLoader.Instance._Qs)
            //{
            //    objectives.Add(q.CurrentObjective.ID);
            //    foreach(var p in q.CurrentSteps)
            //    {
            //        steps.Add(q.CurrentObjective.ID + "\t" + p.GetType().Name);
                    
            //    }
                
            //}

            //var objstr = string.Join("+", objectives);
            //txt.text = objstr;

            //var stepstr = string.Join("_", steps);
            //txtStep.text = stepstr;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

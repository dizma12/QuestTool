using QuestMaker.Data;
using QuestMaker.Data.Steps;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestMaker.Core
{
    public class Quest
    {
        public Quest(QuestSO data)
        {
            if (data == null)
                throw new ArgumentNullException("[Quest] QuestSO data cannot be null");

            _questdata = data;

            //Debug.Log($"Created quest with name= {data.QuestName}");
            if (data.Prerequisites.Level < 5)
            {
                Debug.Log($"Player meets the requirments for quest: {data.QuestName} and level req= {data.Prerequisites.Level}");
                var x = data.Objectives;

                foreach (var item in x)
                {
                    Debug.Log(item.Description);

                    foreach (var item2 in item.Steps)
                        Debug.Log(item2.GetType());
                }
                //foreach (StepCategory pp in Enum.GetValues(typeof(StepCategory)))
                //{

                //}
            }

        }
        private QuestSO _questdata;
    }
}

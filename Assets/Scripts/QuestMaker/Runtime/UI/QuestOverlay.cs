
using QuestMaker.Domain.Steps;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Quests;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace QuestMaker.Runtime.UI
{
    internal class QuestOverlay : MonoBehaviour
    {
        [SerializeField] GameObject _tmp_pref = null;

        private Dictionary<string, TMP_Text> texts;
        private QuestEventBus _questBus = null;

        private void OnEnable()
        {
            _questBus ??= ReferenceManager.Instance.GetReference<GameEventManager>().RequestBus<QuestEventBus>();

            _questBus.OnQuestStarted += HandleQuestStart;
            _questBus.OnQuestCanFinish += HandleQuestCanFinish;
            _questBus.OnQuestCompleted += HandleQuestCompleted;

            texts ??= new();
        }
        private void OnDisable()
        {
            _questBus = null;

            _questBus.OnQuestStarted -= HandleQuestStart;
            _questBus.OnQuestCanFinish -= HandleQuestCanFinish;
            _questBus.OnQuestCompleted -= HandleQuestCompleted;

            texts = null;
        }
        private void HandleQuestStart(Quest quest)
        {
            if(quest == null || texts.ContainsKey(quest.ID)) return;
            
            TMP_Text questText = Instantiate(_tmp_pref, transform).GetComponent<TMP_Text>();

            quest.Changed += HandleQuestChange;
            
            texts.Add(quest.ID, questText);

            BuildQuestDisplayString(quest, questText);
            
        }
        private void HandleQuestCompleted(Quest quest)
        {
            if (quest == null || !texts.TryGetValue(quest.ID, out var text)) return;
            
            texts.Remove(quest.ID);
            quest.Changed -= HandleQuestChange;
            Destroy(text.gameObject);
        }
        private void HandleQuestChange(Quest quest)
        {
            if(!texts.TryGetValue(quest.ID, out var text)) return;
            BuildQuestDisplayString(quest, text);  
        }
        private void HandleQuestCanFinish(Quest quest)
        {
            if (!texts.TryGetValue(quest.ID, out var text)) return;
            text.color = Color.green;
        }
        private void BuildQuestDisplayString(Quest quest, TMP_Text questText)
        {
            questText.text = string.Empty;

            List<string> displayStrings = new();

            foreach (QuestStep step in quest.CurrentSteps)
            {
                displayStrings.Add(step.ProgressText);
            }
            questText.text = $"{quest.ID}: {Environment.NewLine}{string.Join(Environment.NewLine, displayStrings)}";
        }
    }
}

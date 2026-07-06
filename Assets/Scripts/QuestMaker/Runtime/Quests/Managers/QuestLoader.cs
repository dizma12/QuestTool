using QuestMaker.Domain;
using QuestMaker.Domain.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestMaker.Runtime.Quests
{
    public class QuestLoader
    {
        private readonly string _loadingPath = string.Empty;
        private readonly IQuestEventSource _eventBus = null;
        public QuestLoader(IQuestEventSource eventbus ,string path = "Quests")
        {
            if (!string.IsNullOrEmpty(path))
                _loadingPath = path;
            else _loadingPath = "Quests";

            if(eventbus == null)
            {
                ConsoleLogger.LogError(this, "Game Event Bus cannot be null");
                return;
            }
            _eventBus = eventbus;
        }

        /// <summary>
        /// Loads Quests from Resources/Quests and creates a map of ID,Quest.
        /// </summary>
        /// <returns>Null if failed</returns>
        public Dictionary<string, Quest> CreateQuestMap()
        {

            QuestSO[] questSOs = LoadQuests();
            Dictionary<string, Quest> questMap = new();

            foreach (QuestSO questSO in questSOs)
            {
                if (questMap.ContainsKey(questSO.ID))
                {
                    ConsoleLogger.LogError(this, $"Duplicate Quest ID found When creating Map: {questSO.ID}");
                    return null;
                }
                questMap.Add(questSO.ID, new(questSO, _eventBus));
            }
            return questMap;

        }

        private QuestSO[] LoadQuests()
        {
            var allQuests = Resources.LoadAll<QuestSO>(_loadingPath);
            if (allQuests == null || !allQuests.Any())
            {
                ConsoleLogger.Log(this, $"Failed to load quests from path Assets/Resources/{_loadingPath}");
                return null;
            }
            return allQuests;

        }

    }
}
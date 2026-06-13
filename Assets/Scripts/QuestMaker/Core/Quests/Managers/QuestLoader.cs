using QuestMaker.Data;
using QuestMaker.Data.SpecialEvents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestMaker.Core.Quests
{
    public class QuestLoader
    {
        readonly private string _loadingPath;

        public QuestLoader(string path = "Quests")
        {
            if (!string.IsNullOrEmpty(path))
                _loadingPath = path;
            else _loadingPath = "Quests";
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
                    Debug.LogError($"[{GetType()}] Duplicate Quest ID found When creating Map: {questSO.ID}");
                    return null;
                }
                questMap.Add(questSO.ID, new(questSO));

                if (questSO.ID.Equals("EmptyRewards"))
                {
                    Debug.Log(questSO.ID);
                    Debug.Log($"Prereq= {questSO.Prerequisites is null}");
                    Debug.Log($"Rewards= {questSO.Rewards is null}");
                    Debug.Log($"SE= {questSO.SpecialEvent is null}");
                    Debug.Log($"SE= {questSO.Prerequisites.Items.First().Item.Name}");

                }
            }
            return questMap;

        }

        private QuestSO[] LoadQuests()
        {
            var allQuests = Resources.LoadAll<QuestSO>(_loadingPath);
            if (allQuests == null || !allQuests.Any())
            {
                Debug.Log($"Failed to load quests from path Assets/Resources/{_loadingPath}");
                return null;
            }
            return allQuests;

        }

    }
}
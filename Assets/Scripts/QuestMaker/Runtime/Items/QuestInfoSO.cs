using UnityEngine;
namespace QuestMaker
{
    public class QuestInfoSO : ScriptableObject
    {
        public string QuestID { get; set; }
        public string QuestName { get; set; }

        public QuestInfoSO() { }
        public QuestInfoSO(string questID, string questName)
        {
            QuestID = questID;
            QuestName = questName;
        }
    }
}

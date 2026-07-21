
using System.Collections.Generic;

namespace QuestMaker.Runtime.SaveSystem
{
    [System.Serializable]
    internal abstract class SaveDataBase
    {
        public string saveID = string.Empty;
    }

    [System.Serializable]
    internal class PlayerSaveDataBase : SaveDataBase
    {
        public uint level = 1;
        public uint currentExp = 0;
        public uint expToNextLevel = 200;
    }

    [System.Serializable]
    internal class InventorySaveDataBase : SaveDataBase
    {
        public List<ItemSaveData> items = new();
    }

    [System.Serializable]
    internal class QuestsSaveDataBase : SaveDataBase
    {
        public List<QuestSaveData> quests = new();
        public List<string> completedQuestIDs = new();
    }

    [System.Serializable]
    internal class ItemSaveData
    {
        public string itemID = string.Empty;
        public int amount = 0;
    }

    [System.Serializable]
    internal class QuestSaveData
    {
        public string questID = string.Empty;
        public int status = 0;
        public int objectiveIndex = 0;
        public List<StepSaveData> currentSteps = new();
    }

    [System.Serializable]
    internal class StepSaveData
    {
        public int progress = 0;
        public bool isComplete = false;
    }
}
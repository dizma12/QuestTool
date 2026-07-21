
namespace QuestMaker.Runtime.SaveSystem
{
    [System.Serializable]
    internal class SaveFile
    {
        public int version = 1;
        public PlayerSaveDataBase player = null;
        public InventorySaveDataBase inventory = null;
        public QuestsSaveDataBase quests = null;
    }
}
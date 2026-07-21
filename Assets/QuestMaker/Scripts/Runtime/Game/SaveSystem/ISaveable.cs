using QuestMaker.Runtime.SaveSystem;

namespace QuestMaker.Runtime.Saving
{
    internal interface ISaveable
    {
        string SaveID { get; }
        int RestoreOrder { get; }
        SaveDataBase SaveState();
        void RestoreState(SaveDataBase data);
    }
}
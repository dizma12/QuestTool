
namespace QuestMaker.Runtime
{
    public abstract class QuestAcquisitionPrerequisite
    {
        public enum PrerequisiteType
        {
            Level,
            Quest,
            Item
        }

        public PrerequisiteType Prerequisite;
    }

    public class ItemPrerequisite : QuestAcquisitionPrerequisite
    {
        public Item Item;
    }
    public class LevelPrerequisite : QuestAcquisitionPrerequisite
    {
        public int Level;
    }
    public class QuestPrerequisite : QuestAcquisitionPrerequisite
    {
        public string QuestID;
    }
}
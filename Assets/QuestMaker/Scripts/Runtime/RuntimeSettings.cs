

namespace QuestMaker.Runtime
{
    internal static class RuntimeSettings
    {
        public const int MAX_INVENTORY_CAPACITY = 24;


        //player
        public const ushort MAX_PLAYER_LEVEL = 60;
        public const ushort STARTING_NEXT_LEVEL_EXP = 200;
        public const float NEXT_LEVEL_MODIFIER = 1.5f;

        //save 
        public const string FILE_NAME = "savegame.json";
        public const string PLAYER_SAVEID = "PLAYER";
        public const string INVENTORY_SAVEID = "INVENTORY";
        public const string QUESTS_SAVEID = "QUESTS";

    }
}

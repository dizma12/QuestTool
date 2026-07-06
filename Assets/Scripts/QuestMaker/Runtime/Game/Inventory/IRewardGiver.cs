using QuestMaker.Runtime.Quests;


namespace QuestMaker.Runtime.Game
{
    internal interface IRewardGiver
    {
        void GrantRewards(Quest quest);
    }
}

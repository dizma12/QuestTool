using System;

namespace QuestMaker.Runtime.Data
{
    [Serializable]
    public class ItemRewardData : RewardData
    {
        public int Amount { get; set; } = 0;
        public Item Item { get; set; } = null;
    }
}
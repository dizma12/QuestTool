using System;

namespace QuestMaker.Runtime.Data
{
    [Serializable]
    public class ExpRewardData : RewardData
    {
        public float Amount { get; set; } = 0;
    }
}
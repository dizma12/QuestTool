using System;

namespace QuestMaker.Runtime.Data
{
    [Serializable]
    public class CurrencyRewardData : RewardData
    {
        public float Amount { get; set; } = 0;
    }
}
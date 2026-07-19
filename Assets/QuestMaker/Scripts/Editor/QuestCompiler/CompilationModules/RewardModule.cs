using QuestMaker.Domain;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace QuestMaker.Editor.CompilationModules
{
    internal class RewardModule : IQuestModuleBuilder, IExpModule, IItemModule, IAbilityModule, IReputationModule
    {
        private int _exp = 0;
        private readonly List<ItemStack> _items = new();
        private readonly List<ReputationFaction> _reps = new ();
        private readonly List<string> _abilities = new ();
        public void Build(QuestSO quest)
        {
            RewardData data = new(_exp, _items.ToArray() , _abilities.ToArray(), _reps.ToArray());
            quest.Rewards = data;
        }

        public void SetAbility(string abilityId)
        {
            if(string.IsNullOrEmpty(abilityId))
                return;

            _abilities.Add(abilityId);
        }

        public void SetExp(int amount)
        {
            if(amount == 0) amount = 1;
            else if(amount < 0) amount = Mathf.Abs(amount);
            _exp = amount;
        }

        public void SetItem(Item item, int amount = 1)
        {
            if (item == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add Item coz its null");

            if (amount == 0) amount = 1;
            else if (amount < 0) amount = Mathf.Abs(amount);

            _items.Add(new(item, amount));
        }

        public void SetReputationFaction(ReputationFaction rep)
        {
            if(string.IsNullOrEmpty(rep.FactionID) || rep.Amount < 1)
                throw new NullReferenceException($"[{GetType().Name}] Cannot add Reputation coz its null or less than 0");

            _reps.Add(rep);
        }
    }
}

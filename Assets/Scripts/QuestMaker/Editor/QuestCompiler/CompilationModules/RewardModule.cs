using QuestMaker.Data;
using System;
using System.Collections.Generic;


namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class RewardModule : IQuestModuleBuilder, IExpModule, IItemModule, IAbilityModule
    {
        private int _exp = 0;
        private readonly List<ItemAmount> _items = new();
        private readonly List<string> _abilities = new ();
        public void Build(QuestSO quest)
        {
            RewardData data = new(_exp, _items.ToArray() , _abilities.ToArray());
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
            if (amount < 1 || _exp >= amount) return;
            _exp = amount;
        }

        public void SetItem(Item item, int amount = 1)
        {
            if (item == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add Item coz its null");
            if (amount < 1) return;

            _items.Add(new() { Item = item, Amount = amount });
        }


    }
}

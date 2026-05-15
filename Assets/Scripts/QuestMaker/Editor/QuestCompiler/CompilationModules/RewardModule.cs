using QuestMaker.Runtime;
using QuestMaker.Runtime.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class RewardModule : IQuestModuleBuilder, IExpModule, IItemModule
    {
        private int _exp = 0;
        private int _itemAmount = 0;
        private Item _item = null;

        public void Build(QuestSO quest)
        {
            RewardData data = new(_exp, new List<Item> { _item});
            quest.AddRewards(data);
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

            _item = item;
            _itemAmount = amount;
        }
    }
}

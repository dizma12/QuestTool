using QuestMaker.Data;
using QuestMaker.Data.Steps;
using QuestMaker.Runtime.Events.Handlers;
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Game.Events;
using QuestMaker.Runtime.StepsAndObjectives;
using System;
using UnityEngine;

namespace QuestMaker.Core
{
    internal class LootQuestStep : QuestStep
    {
        private ItemAmount _item = default;
        private int _currentAmount = 0;
        private GameEventHandler _eventHandler = null;
        public LootQuestStep(QuestStepData data) : base(data) { }


        private void HandleItemCollection(string itemID)
        {
            if(!_item.Item.ID.Equals(itemID))
                return;

            _currentAmount++;
            Debug.Log($"[LootQuestStep] {_currentAmount}/{_item.Amount} {_item.Item.Name} collected");

            if (Validate())
                FinishStep();
        }

        public override void Initialize(QuestStepData data)
        {
            if(data == null)
            {
                Debug.LogError($"[LootQuestStep] {data?.GetType()} is null");
                return;
            }
            if (data is not LootStepData lootData)
            {
                Debug.LogError($"[LootQuestStep] Expected LootStepData, got {data?.GetType()}");
                return;
            }
            if (lootData.Loot.Item == null || lootData.Loot.Amount <= 0)
            {
                Debug.LogError($"[LootQuestStep] Invalind ItemAmount pair with item: {lootData.Loot.Item} and amount: {lootData.Loot.Amount}");
                return;
            }

            _item = lootData.Loot;

            _eventHandler = ReferenceManager.Instance
                .GetReference<GameEventManager>()
                .GetEventHandler<GameEventHandler>();

            _eventHandler.OnItemCollected += HandleItemCollection;
        }

        public override bool Validate() => _currentAmount >= _item.Amount;

        protected override void FinishStep()
        {
            _eventHandler.OnItemCollected -= HandleItemCollection;

            base.FinishStep();
        }
    }
}
using QuestMaker.Data;
using QuestMaker.Data.Steps;
using QuestMaker.Runtime.Events.Handlers;
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Game.Events;
using System;
using UnityEngine;


namespace QuestMaker.Runtime.StepsAndObjectives
{
    internal class CraftQuestStep : QuestStep
    {
            
        private ItemAmount _item = default;
        private int _currentAmount = 0;
        private GameEventHandler _eventHandler = null;
        public CraftQuestStep(QuestStepData data) : base(data) { }
    
    

        public override void Initialize(QuestStepData data)
        {
            if (data == null)
            {
                Debug.LogError($"[LootQuestStep] {data?.GetType()} is null");
                return;
            }
            if (data is not CraftStepData craftData)
            {
                Debug.LogError($"[CraftQuestStep] Expected CraftStepData, got {data?.GetType()}");
                return;
            }
            if (craftData.CraftableItem.Item == null || craftData.CraftableItem.Amount <= 0)
            {
                Debug.LogError($"[CraftQuestStep] Invalind ItemAmount pair with item: {craftData.CraftableItem.Item} and amount: {craftData.CraftableItem.Amount}");
                return;
            }
            _item = craftData.CraftableItem;

            _eventHandler = ReferenceManager.Instance
                .GetReference<GameEventManager>()
                .GetEventHandler<GameEventHandler>();

            _eventHandler.OnItemCrafted += HandleItemCraft;
        }

        private void HandleItemCraft(string itemID)
        {
            if (!_item.Item.ID.Equals(itemID)) return;

            _currentAmount++;
            Debug.Log($"[CraftQuestStep] {_currentAmount}/{_item.Amount} {_item.Item.Name} crafted");

            if(Validate())
                FinishStep();

        }

        public override bool Validate() => _currentAmount >= _item.Amount;

        protected override void FinishStep()
        {
            _eventHandler.OnItemCrafted -= HandleItemCraft;

            base.FinishStep();
        }

    }
}
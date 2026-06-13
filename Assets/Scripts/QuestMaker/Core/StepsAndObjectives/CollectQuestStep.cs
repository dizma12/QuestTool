using QuestMaker.Data;
using QuestMaker.Data.Steps;
using QuestMaker.Runtime.Events.Handlers;
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Game.Events;
using UnityEngine;

namespace QuestMaker.Runtime.StepsAndObjectives
{
    internal class CollectQuestStep : QuestStep
    {
        private ItemAmount _collectableItem;
        private int _currentAmount = 0;
        private GameEventHandler _eventHandler = null;
        public CollectQuestStep(QuestStepData data) : base(data) { }


        public override void Initialize(QuestStepData data)
        {
            if (data == null)
            {
                Debug.LogError($"[CollectQuestStep] QuestStepData is null");
                return;
            }
            if (data is not CollectStepData collectData)
            {
                Debug.LogError($"[CollectQuestStep] Expected CollectStepData, got {data?.GetType()}");
                return;
            }
            if (collectData.CollectableItem.Item == null || collectData.CollectableItem.Amount <= 0)
            {
                Debug.LogError($"[LootQuestStep] Invalind ItemAmount pair with item: {collectData.CollectableItem.Item} and amount: {collectData.CollectableItem.Amount}");
                return;
            }
            _collectableItem = collectData.CollectableItem;

            _eventHandler = ReferenceManager.Instance
                            .GetReference<GameEventManager>()
                            .GetEventHandler<GameEventHandler>();

            _eventHandler.OnItemCollected += HandleItemCollection;

        }

        private void HandleItemCollection(string itemID)
        {
            if (!_collectableItem.Item.ID.Equals(itemID))
                return;

            _currentAmount++;
            Debug.Log($"[CollectQuestStep] {_currentAmount}/{_collectableItem.Amount} {_collectableItem.Item.Name} collected");

            if (Validate())
                FinishStep();
        }
        public override bool Validate() => _currentAmount >= _collectableItem.Amount;
      
    }
}
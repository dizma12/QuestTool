using QuestMaker.Domain;
using QuestMaker.Domain.Steps;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Quests;
using System.Collections.Generic;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    internal class DummyCollectableSpawner : MonoBehaviour
    {
        [Tooltip("If true spawns a single pickup that grants the step's full amount. " +
            "If false spawns enough pickups to cover the step amount -> ceil(amount / ItemsPerPickup)")]
        [SerializeField] private bool _spawnSingle = true;
        [SerializeField] private List<Transform> _spawns = null;

        private QuestEventBus _questBus = null;
        private int _spawnIndex = 0;

        private void OnEnable()
        {
            if (_spawns == null || _spawns.Count == 0)
            {
                ConsoleLogger.LogWarning(this, "No spawn points assigned");
                return;
            }
            _questBus = ReferenceManager.Instance.RequestReference<GameEventManager>().RequestBus<QuestEventBus>();
            if (_questBus == null)
            {
                ConsoleLogger.LogError(this, "Failed to retrieve Quest Event Bus Reference");
                return;
            }
            _questBus.OnQuestObjectiveChanged += HandleObjectiveChanged;
        }


        private void OnDisable()
        {
            if (_questBus != null)
                _questBus.OnQuestObjectiveChanged -= HandleObjectiveChanged;
        }

        private void HandleObjectiveChanged(Quest quest)
        {
            ConsoleLogger.Log(this, $"Found collectable step in quest {quest.ID}");
            foreach (CollectStepData step in quest.GetCurrentStepsOfType<CollectStepData>())
            {
                SpawnCollectables(step.CollectableItem);
            }
        }

        private void SpawnCollectables(ItemStack stack)
        {

            if (stack.Item == null || stack.Amount <= 0)
            {
                ConsoleLogger.LogWarning(this, "Collect step has no valid item to spawn");
                return;
            }
            if (stack.Item.Prefab == null)
            {
                ConsoleLogger.LogWarning(this, $"Item: [{stack.Item.Name}] has no prefab assigned");
                return;
            }
            if (!stack.Item.Prefab.TryGetComponent(out CollectableItem collectable))
            {
                ConsoleLogger.LogError(this, $"The prefab of item: [{stack.Item.Name}] is not a CollectableItem");
                return;
            }


            int spawnCount = 1;

            if (!_spawnSingle)                     //Use Max coz if for some reason the prefab has ItemsPerPickup = 0 will throw an error
                spawnCount = Mathf.CeilToInt((float)stack.Amount / Mathf.Max(1, collectable.ItemsPerPickup));

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnCollectible(stack.Item.Prefab, ref stack);
            }
        }

        private void SpawnCollectible(GameObject prefab, ref ItemStack stack)
        {
            Vector2 spawnPosition = _spawns[_spawnIndex % _spawns.Count].position;

            _spawnIndex++;

            GameObject go = Instantiate(prefab, spawnPosition, Quaternion.identity);

            if(go.TryGetComponent(out CollectableItem instance))
            {
                instance.Item = stack.Item;

                if (_spawnSingle) instance.ItemsPerPickup = stack.Amount;
            }
                
        }
    }
}
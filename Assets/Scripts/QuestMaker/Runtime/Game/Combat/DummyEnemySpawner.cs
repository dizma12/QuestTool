using QuestMaker.Domain;
using QuestMaker.Domain.Steps;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Quests;
using System.Collections.Generic;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    internal class DummyEnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _enemyPrefab = null;
        [SerializeField] private List<Transform> _spawns = null;

        private QuestEventBus _questBus = null;
        private int _spawnIndex = 0;

        private void Start()
        {
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
            foreach (SlayStepData step in quest.GetCurrentStepsOfType<SlayStepData>())
            {
                SpawnEnemy(step.EnemyID, step.Amount);
            }
        }

        private void SpawnEnemy(string enemyID, int amount)
        {
            if (_enemyPrefab == null || string.IsNullOrEmpty(enemyID) || amount <= 0) return;

            Vector2 spawnPosition = _spawns[_spawnIndex % _spawns.Count].position;

            _spawnIndex++;
          
            Enemy enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.InitializeEnemy(enemyID, System.Convert.ToUInt16(amount));

            if (enemy.TryGetComponent(out SpriteRenderer renderer))
                renderer.color = Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.7f, 1f);
        }
    }
}

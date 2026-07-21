using QuestMaker.Domain;
using QuestMaker.Domain.Helpers;
using QuestMaker.Runtime.Events;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        [SerializeField, ReadOnlyInspector] private float _health = 1f;
        [SerializeField, ReadOnlyInspector] private uint _eventRepeat = 1;

        [SerializeField] private string _id = string.Empty;

        private GameEventBus _gameEventBus = null;
        private void Start()
        {
            if (string.IsNullOrEmpty(_id))
            {
                ConsoleLogger.LogWarning(this, "Enemy ID cannot be null");
                return;
            }

            _gameEventBus = ReferenceManager.Instance.RequestReference<GameEventManager>().RequestBus<GameEventBus>();
            if (_gameEventBus == null)
            {
                ConsoleLogger.LogWarning(this, "Failed to find reference of Game Event Bus");
                return;
            }

        }
        public void InitializeEnemy(string id, ushort timesToRepeat,float health = 1f )
        {
            _id = id;
            _health = health;
            _eventRepeat = timesToRepeat;
        }
        public void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health < 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            for (int i = 0; i < _eventRepeat; i++)
            {
                _gameEventBus.FireEnemyKilled(_id);
            }
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    [RequireComponent(typeof(CircleCollider2D))]
    internal class PlayerHitbox : MonoBehaviour
    {
        [SerializeField] private float _damage = 5f;
        private CircleCollider2D _hitBoxCollider = null;
        private void Start()
        {
            _hitBoxCollider = GetComponent<CircleCollider2D>();
            _hitBoxCollider.isTrigger = true;
            _hitBoxCollider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamagable damagable))
                damagable.TakeDamage(_damage);
        }
    }
}



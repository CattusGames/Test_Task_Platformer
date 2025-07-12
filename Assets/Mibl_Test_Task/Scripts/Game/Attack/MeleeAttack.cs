using UnityEngine;

namespace Mibl_Test_Task.Scripts.Game.Attack
{
    public class MeleeAttack : CharacterAttack
    {
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float _attackRadius = 0.5f;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private int _damage = 1;

        protected override void PerformAttack(LayerMask targetLayerMask)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _enemyLayer);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_damage);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_attackPoint != null)
                Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
        }
    }
}
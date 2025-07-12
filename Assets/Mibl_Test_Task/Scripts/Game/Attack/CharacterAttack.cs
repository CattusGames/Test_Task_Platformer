using UnityEngine;
using Zenject;

namespace Mibl_Test_Task.Scripts.Game.Attack
{
    public abstract class CharacterAttack : MonoBehaviour
    {
        [SerializeField] protected float _attackCooldown = 0.5f;

        protected float _lastAttackTime;

        public virtual void Attack(LayerMask targetLayerMask)
        {
            if (Time.time - _lastAttackTime < _attackCooldown)
                return;

            _lastAttackTime = Time.time;
            PerformAttack(targetLayerMask);
        }

        protected abstract void PerformAttack(LayerMask targetLayerMask);
    }
}
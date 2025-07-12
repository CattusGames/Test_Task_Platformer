using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mibl_Test_Task.Scripts.Game
{
    public class EnemyRadar : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D _circle;
        [SerializeField] private LayerMask _playerLayer;
        
        private bool _playerInRange;

        public event Action OnPlayerDetected;
        public event Action OnPlayerLost;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
                OnPlayerDetected?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
                OnPlayerLost?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            if (_circle == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _circle.radius);
        }
    }
}
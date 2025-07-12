using System;
using UnityEngine;

namespace Mibl_Test_Task.Scripts.Game.Attack.Projectile
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField] private int _damage = 1;
        
        private Rigidbody2D _rb;
        private float _lifeTime = 3f;
        private float _timer;

        public ProjectileType Type { get; private set; }
        private System.Action<Projectile> _onDespawn;
        private LayerMask _targetLayer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(ProjectileType type, System.Action<Projectile> onDespawn)
        {
            Type = type;
            _onDespawn = onDespawn;
        }

        public void Launch(LayerMask targetLayer,Vector2 direction, float speed)
        {
            _targetLayer = targetLayer;
            gameObject.SetActive(true);
            _timer = _lifeTime;
            _rb.velocity = direction.normalized * speed;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                ReturnToPool();
            }
        }

        public void ReturnToPool()
        {
            _rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
            _onDespawn?.Invoke(this);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == _targetLayer)
            {
                other.gameObject.GetComponent<Health>().TakeDamage(_damage);
                ReturnToPool();
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                ReturnToPool();
            }
        }
    }

}
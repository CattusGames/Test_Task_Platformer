using System.Collections.Generic;
using UnityEngine;

namespace Mibl_Test_Task.Scripts.Game.Attack.Projectile
{
    public class ProjectilePool
    {
        private readonly Queue<Projectile> _pool = new();
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        private readonly ProjectileType _type;

        public ProjectilePool(GameObject prefab, ProjectileType type, Transform parent, int initialCount = 10)
        {
            _prefab = prefab;
            _type = type;
            _parent = parent;
            for (int i = 0; i < initialCount; i++)
            {
                AddNewProjectile();
            }
        }

        private void AddNewProjectile()
        {
            var instance = GameObject.Instantiate(_prefab, _parent);
            var projectile = instance.GetComponent<Projectile>();
            projectile.Initialize(_type, ReturnToPool);
            projectile.gameObject.SetActive(false);
            _pool.Enqueue(projectile);
        }

        public Projectile Get()
        {
            if (_pool.Count == 0)
                AddNewProjectile();

            return _pool.Dequeue();
        }

        private void ReturnToPool(Projectile projectile)
        {
            _pool.Enqueue(projectile);
        }
    }
}
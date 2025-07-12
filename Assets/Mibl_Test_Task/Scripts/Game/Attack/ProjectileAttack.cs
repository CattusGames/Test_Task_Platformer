using System;
using System.Threading.Tasks;
using Mibl_Test_Task.Scripts.Game.Attack.Projectile;
using Services.Factories.GameFactory;
using UnityEngine;
using Zenject;

namespace Mibl_Test_Task.Scripts.Game.Attack
{
    public class ProjectileAttack : CharacterAttack
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private int _maxBullets;
        [SerializeField] private float _projectileSpeed = 5f;
        [SerializeField] private ProjectileType _projectileType;
        
        private int _currentBulletsCount;
        private IGameFactory _gameFactory;

        public event Action OnAttack;
        
        public int CurrentBulletsCount => _currentBulletsCount;
        public int MaxBulletsCount => _maxBullets;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public async Task Initialize()
        {
            _currentBulletsCount = _maxBullets;
            await _gameFactory.LoadProjectile(_projectileType);
        }

        protected override void PerformAttack(LayerMask targetLayerMask)
        {
            var projectile = _gameFactory.GetProjectile(_projectileType);
            projectile.transform.position = _firePoint.position;
            if(transform.localScale.x > 0) 
                projectile.Launch(targetLayerMask,_firePoint.right, _projectileSpeed);
            else
                projectile.Launch(targetLayerMask,-_firePoint.right, _projectileSpeed);
            
            _currentBulletsCount--;
            
            if(_currentBulletsCount == 0)
                _currentBulletsCount = _maxBullets;
            
            OnAttack?.Invoke();
        }
    }
}
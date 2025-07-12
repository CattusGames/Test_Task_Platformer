using System;
using Mibl_Test_Task.Scripts.Game.Attack;
using Mibl_Test_Task.Scripts.Services.GameplayUIService;
using Services.Factories.GameFactory;
using Services.Level;
using Services.PersistenceProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.Bars
{
    public class BulletCounter: MonoBehaviour
    {
        [SerializeField] private Text _text;
        private IGameFactory _gameFactory;
        private ILevelDescriptionService _levelDescriptionService;
        private IPersistenceProgressService _persistenceProgressService;
        private ProjectileAttack _playerProjectileAttack;

        [Inject]
        public void Construct(IGameFactory gameFactory, ILevelDescriptionService levelDescriptionService, IPersistenceProgressService persistenceProgressService)
        {
            _persistenceProgressService = persistenceProgressService;
            _levelDescriptionService = levelDescriptionService;
            _gameFactory = gameFactory;
        }

        public void Initialize()
        {
            _playerProjectileAttack = _gameFactory.Player.GetComponent<ProjectileAttack>();
            _playerProjectileAttack.OnAttack += ChangeValue;
            ChangeValue();
        }

        private void ChangeValue()
        {
            _text.text = _playerProjectileAttack.CurrentBulletsCount + "/" + _playerProjectileAttack.MaxBulletsCount;  
        }

        private void OnDisable()
        {
            _playerProjectileAttack.OnAttack -= ChangeValue;
        }
    }
}
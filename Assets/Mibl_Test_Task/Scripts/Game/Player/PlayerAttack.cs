using System;
using Mibl_Test_Task.Scripts.Game.Attack;
using Mibl_Test_Task.Scripts.Services.GameplayUIService;
using UnityEngine;
using Zenject;

namespace Mibl_Test_Task.Scripts.Game
{
    public class PlayerAttack: MonoBehaviour
    {
        [SerializeField] private CharacterAttack _characterAttack;
        private IGameplayUIService _gameplayUIService;

        [Inject]
        public void Construct(IGameplayUIService gameplayUIService)
        {
            _gameplayUIService = gameplayUIService;
        }
        
        private void OnEnable()
        {
            _gameplayUIService.OnInputDown += HandleInput;
        }

        private void AttackPressed()
        {
            _characterAttack.Attack(LayerMask.NameToLayer("Enemy"));
        }
        
        private void HandleInput(GameplayInputType obj)
        {
            if (obj == GameplayInputType.Attack)
            {
                AttackPressed();
            }
        }
        
        private void OnDisable()
        {
            _gameplayUIService.OnInputDown -= HandleInput;
        }
        
    }
}
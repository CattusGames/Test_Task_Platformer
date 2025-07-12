using System;
using Mibl_Test_Task.Scripts.Game.Attack;
using UnityEngine;

namespace Mibl_Test_Task.Scripts.Game
{
    public class EnemyAttack: MonoBehaviour
    {
        [SerializeField] private CharacterAttack _characterAttack;
        [SerializeField] private EnemyRadar _radar;
        private bool _startAttackLoop;

        private void OnEnable()
        {
            _radar.OnPlayerDetected += StartAttack;
            _radar.OnPlayerLost += StopAttack;
        }

        private void Update()
        {
            if (!_startAttackLoop)
                return;
            
            _characterAttack.Attack(LayerMask.NameToLayer("Player"));
        }

        private void StopAttack()
        {
            _startAttackLoop = false;
        }

        private void StartAttack()
        {
            _startAttackLoop = true;
        }

        private void OnDisable()
        {
            _radar.OnPlayerDetected -= StartAttack;
            _radar.OnPlayerLost -= StopAttack;
            _startAttackLoop = false;
        }
    }
}
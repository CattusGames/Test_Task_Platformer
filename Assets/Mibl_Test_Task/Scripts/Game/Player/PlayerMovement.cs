using System;
using Mibl_Test_Task.Scripts.Game.Movement;
using Mibl_Test_Task.Scripts.Game.Player;
using Mibl_Test_Task.Scripts.Services.GameplayUIService;
using UnityEngine;
using Zenject;

namespace Mibl_Test_Task.Scripts.Game
{
    public class PlayerMovement : CharacterMovement
    {
        [SerializeField] private JumpSquashStretchEffect _jumpEffect;
        [SerializeField] private float _fallMultiplier = 2.5f;
        [SerializeField] private float _lowJumpMultiplier = 2f;
        
        private IGameplayUIService _gameplayUIService;

        [Inject]
        public void Construct(IGameplayUIService gameplayUIService)
        {
            _gameplayUIService = gameplayUIService;
        }

        private void OnEnable()
        {
            _gameplayUIService.OnInputDown += HandleInputDown;
            OnGroundedChanged += OnGroundedStateChanged;
        }

        private void OnGroundedStateChanged(bool obj)
        {
            if (obj)
                _jumpEffect?.PlaySquash();
            else
                _jumpEffect?.PlayStretch();
        }

        private void Update()
        {
            base.Update();
            HandleBetterJump();
        }
        
        private void HandleBetterJump()
        {
            if (_rb.velocity.y < 0)
            {
                _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
            }
            else if (_rb.velocity.y > 0 && !_gameplayUIService.IsHeld(GameplayInputType.Jump))
            {
                _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
        
        private void HandleInputDown(GameplayInputType input)
        {
            switch (input)
            {
                case GameplayInputType.Jump:
                    Jump();
                    break;
                case GameplayInputType.MoveLeft:
                    Move(-1f);
                    break;
                case GameplayInputType.MoveRight:
                    Move(1f);
                    break;
                case GameplayInputType.MoveReleased:
                    Move(0f);
                    break;
            }
        }

        private void OnDisable()
        {
            _gameplayUIService.OnInputDown -= HandleInputDown;
            OnGroundedChanged -= OnGroundedStateChanged;
        }
    }
}
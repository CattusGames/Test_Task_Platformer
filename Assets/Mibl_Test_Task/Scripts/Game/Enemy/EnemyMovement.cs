using Game;
using Mibl_Test_Task.Scripts.Game.Movement;
using Services.Factories.GameFactory;
using UnityEngine;
using Zenject;

namespace Mibl_Test_Task.Scripts.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : CharacterMovement
    {
        [Header("Raycast Check")]
        [SerializeField] private Transform _frontCheck;
        [SerializeField] private EnemyRadar _radar;

        private Vector3 _startPos;
        private bool _movingRight = true;
        private float _patrolRange;
        private PatrolType _patrolType;
        private bool _isInitialized;
        private float _directionWhenStuck = 1f;

        private bool _isChasingPlayer = false;
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Initialize(EnemySpawnSpot enemySpawnSpot)
        {
            _startPos = transform.position;
            _patrolType = enemySpawnSpot.PatrolType;
            _patrolRange = enemySpawnSpot.PatrolRange;

            _radar.OnPlayerDetected += OnPlayerDetected;

            _isInitialized = true;
        }

        private void OnPlayerDetected()
        {
            _isChasingPlayer = true;
        }

        private void Update()
        {
            if (!_isInitialized)
                return;

            GravityFallIfNoGround();

            if (_isChasingPlayer && _gameFactory.Player != null)
            {
                ChasePlayer();
                return;
            }

            switch (_patrolType)
            {
                case PatrolType.SimpleDistance:
                    PatrolSimpleDistance();
                    break;
                case PatrolType.EdgeAware:
                    if (!IsGroundAhead())
                        FlipDirection();

                    MoveByDirection(_movingRight ? 1 : -1);
                    break;

                case PatrolType.ObstacleAware:
                    if (IsWallAhead())
                        FlipDirection();

                    MoveByDirection(_movingRight ? 1 : -1);
                    break;
            }
        }
        
        private void GravityFallIfNoGround()
        {
            _rb.gravityScale = !IsGrounded() ? 3f : 1f;
        }

        private void ChasePlayer()
        {
            if (_gameFactory.Player == null)
                return;

            Vector3 playerPos = _gameFactory.Player.transform.position;
            Vector3 myPos = transform.position;

            float xDistance = playerPos.x - myPos.x;
            float yDistance = playerPos.y - myPos.y;

            float directionToPlayer = Mathf.Sign(xDistance);
            
            if (Mathf.Abs(yDistance) > 0.1f)
            {
                MoveByDirection((int)_directionWhenStuck);

                if (IsWallAhead())
                {
                    _directionWhenStuck *= -1;
                }
                
                if ((_directionWhenStuck > 0 && !_movingRight) || (_directionWhenStuck < 0 && _movingRight))
                {
                    FlipDirection();
                }
            }
            else
            {
                MoveByDirection((int)directionToPlayer);

                if ((directionToPlayer > 0 && !_movingRight) || (directionToPlayer < 0 && _movingRight))
                {
                    FlipDirection();
                }
            }
        }
        
        private void PatrolSimpleDistance()
        {
            float currentDistance = transform.position.x - _startPos.x;

            if (_movingRight && currentDistance >= _patrolRange)
            {
                FlipDirection();
            }
            else if (!_movingRight && currentDistance <= -_patrolRange)
            {
                FlipDirection();
            }

            MoveByDirection(_movingRight ? 1 : -1);
        }

        private void FlipDirection()
        {
            _movingRight = !_movingRight;
            Flip();
        }

        private bool IsGroundAhead()
        {
            return Physics2D.Raycast(_frontCheck.position, Vector2.down, 1f, _groundMask);
        }

        private bool IsWallAhead()
        {
            Vector2 direction = _movingRight ? Vector2.right : Vector2.left;
            return Physics2D.Raycast(_frontCheck.position, direction, 0.1f, _groundMask);
        }
    }
}

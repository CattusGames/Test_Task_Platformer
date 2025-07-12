using System;
using UnityEngine;

namespace Mibl_Test_Task.Scripts.Game.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class CharacterMovement : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _rb;
        
        [Header("Movement Settings")]
        [SerializeField] protected float _moveSpeed = 3f;
        [SerializeField] protected float _jumpForce = 5f;

        [Header("Ground Check")]
        [SerializeField] protected LayerMask _groundMask;
        [SerializeField] protected Transform _groundCheck;
        
        protected bool _facingRight = true;
        
        private bool _wasGrounded;
        
        public event Action<bool> OnGroundedChanged;
        
        protected void Move(float direction)
        {
            _rb.velocity = new Vector2(direction * _moveSpeed, _rb.velocity.y);
            UpdateFacing(direction);
        }
        
        protected void MoveByDirection(int direction)
        {
            _rb.velocity = new Vector2(direction * _moveSpeed, _rb.velocity.y);
        }

        protected void UpdateFacing(float direction)
        {
            if (direction > 0 && !_facingRight)
                Flip();
            else if (direction < 0 && _facingRight)
                Flip();
        }

        protected void Flip()
        {
            _facingRight = !_facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        protected virtual void Update()
        {
            HandleGroundCheck();
        }
        
        private void HandleGroundCheck()
        {
            bool isGrounded = IsGrounded();
            if (isGrounded != _wasGrounded)
            {
                OnGroundedChanged?.Invoke(isGrounded);
                _wasGrounded = isGrounded;
            }
        }

        public bool IsGrounded()
        {
            return Physics2D.Raycast(_groundCheck.position, Vector2.down, 0.3f, _groundMask);
        }

        protected void Jump()
        {
            if (IsGrounded())
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            }
        }
    }
}
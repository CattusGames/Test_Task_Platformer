using System.Collections;
using Mibl_Test_Task.Scripts.Game.Movement;
using UnityEngine;

namespace Mibl_Test_Task.Scripts.Game.Visual
{
    public class FakeShadow : MonoBehaviour
    {
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private SpriteRenderer _shadowRenderer;
        [SerializeField] private float _fadeDuration = 0.2f;

        private Coroutine _fadeCoroutine;
        private Color _originalColor;

        private void Awake()
        {
            if (_shadowRenderer == null)
                _shadowRenderer = GetComponent<SpriteRenderer>();

            _originalColor = _shadowRenderer.color;
        }

        private void OnEnable()
        {
            _characterMovement.OnGroundedChanged += HandleGroundedChanged;
        }

        private void OnDisable()
        {
            _characterMovement.OnGroundedChanged -= HandleGroundedChanged;
        }

        private void HandleGroundedChanged(bool isGrounded)
        {
            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);

            float targetAlpha = isGrounded ? 1f : 0f;
            _fadeCoroutine = StartCoroutine(FadeTo(targetAlpha));
        }

        private IEnumerator FadeTo(float targetAlpha)
        {
            float startAlpha = _shadowRenderer.color.a;
            float time = 0f;

            while (time < (targetAlpha == 0?_fadeDuration/1.5f:_fadeDuration))
            {
                time += Time.deltaTime;
                float t = time / _fadeDuration;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);

                Color newColor = _originalColor;
                newColor.a = newAlpha;
                _shadowRenderer.color = newColor;

                yield return null;
            }
            
            Color finalColor = _originalColor;
            finalColor.a = targetAlpha;
            _shadowRenderer.color = finalColor;
        }
    }
}
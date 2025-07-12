using System.Collections;
using UnityEngine;

namespace Mibl_Test_Task.Scripts.Game.Player
{
    public class JumpSquashStretchEffect : MonoBehaviour
    {
        [SerializeField] private Transform _visual;
        [SerializeField] private float _squashY = 0.8f;
        [SerializeField] private float _stretchY = 1.2f;
        [SerializeField] private float _duration = 0.1f;

        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = _visual.localScale;
        }

        public void PlayStretch()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateScaleY(_stretchY));
        }

        public void PlaySquash()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateScaleY(_squashY));
        }

        public void ResetScale()
        {
            _visual.localScale = _originalScale;
        }

        private IEnumerator AnimateScaleY(float targetY)
        {
            float elapsed = 0;
            Vector3 from = _visual.localScale;
            Vector3 to = new Vector3(_originalScale.x, targetY, _originalScale.z);

            while (elapsed < _duration)
            {
                _visual.localScale = Vector3.Lerp(from, to, elapsed / _duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            elapsed = 0;
            
            while (elapsed < _duration)
            {
                _visual.localScale = Vector3.Lerp(to, _originalScale, elapsed / _duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            _visual.localScale = _originalScale;
        }
    }
}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Window;

namespace Infrastructure
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        private const float Delay = 0.5f;
    
        [SerializeField] private GameObject _logo;
        [SerializeField] private Button _retryLoadingButton;
        [SerializeField] private RectTransform _container;
        [SerializeField] private Image _background;
        [SerializeField] private Image _firstLoadBackground;
        [SerializeField] private float _moveUpSpeed = 20f;
        [SerializeField] private float _timeStep = 0.03f;
        [SerializeField] private ProgressBar _progressBar;
        
        private Coroutine _hideCoroutine;
        private Action _onContinueClick;

        public Status Current { get; private set; } = Status.Hided;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            gameObject.SetActive(false);
            _retryLoadingButton.onClick.AddListener(OnContinueClick);
        }

        public void Show() => Show(_background);
        
        public void ShowFirst() => Show(_firstLoadBackground);

        private void Show(Image activeBackground)
        {
            _background.gameObject.SetActive(false);
            _firstLoadBackground.gameObject.SetActive(false);

            activeBackground.gameObject.SetActive(true);
            
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }

            _container.anchoredPosition = Vector2.zero;
            gameObject.SetActive(true);
            _progressBar.gameObject.SetActive(false);
            Current = Status.Showed;
        }

        public void ShowProgress(float progress)
        {
            _progressBar.gameObject.SetActive(true);
            _progressBar.DrawBar(progress);
            Current = Status.Showed;
        }

        private void OnContinueClick()
        {
            _onContinueClick?.Invoke();
            _logo.SetActive(true);
        }

        public void Hide()
        {
            if(_hideCoroutine != null || Current == Status.Hided)
                return;
            
            _hideCoroutine = StartCoroutine(GoUp());
        }

        private IEnumerator GoUp()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSecondsRealtime(Delay / 2);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSecondsRealtime(Delay / 2);
            yield return new WaitForEndOfFrame();

            while (_container.anchoredPosition.y < _container.rect.height)
            {
                MoveImageUp();
                yield return new WaitForSecondsRealtime(_timeStep);
            }

            Current = Status.Hided;
            _hideCoroutine = null;
            gameObject.SetActive(false);
        }

        private void MoveImageUp()
        {
            Vector2 anchoredPosition = _container.anchoredPosition;

            anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + _moveUpSpeed);

            _container.anchoredPosition = anchoredPosition;
        }
        
        public enum Status 
        {
            Hided,
            Showed,
        }
    }
}
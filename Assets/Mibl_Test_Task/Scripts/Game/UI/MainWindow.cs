using System;
using Game.UI.Bars;
using Mibl_Test_Task.Scripts.Services.GameplayUIService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class MainWindow: MonoBehaviour
    {
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _shootButton;
        [SerializeField] private Button _pauseButton;
        
        [SerializeField] private BulletCounter _bulletCounter;
        [SerializeField] private KillCounter _killCounter;
        [SerializeField] private DistanceCounter _distanceCounter;
        
        private IGameplayUIService _gameplayUIService;

        [Inject]
        public void Construct(IGameplayUIService gameplayUIService)
        {
            _gameplayUIService = gameplayUIService;
        }

        public void Initialize()
        {
            _bulletCounter.Initialize();
            _killCounter.Initialize();
            _distanceCounter.Initialize();
        }
        
        private void OnEnable()
        {
            AddPressAndReleaseListener(_jumpButton, GameplayInputType.Jump);
            AddPressAndReleaseListener(_shootButton, GameplayInputType.Attack);
            AddPressAndReleaseListener(_pauseButton, GameplayInputType.Pause);
    
            AddPressAndReleaseListener(_leftButton, GameplayInputType.MoveLeft);
            AddPressAndReleaseListener(_rightButton, GameplayInputType.MoveRight);
        }
        

        private void AddPressAndReleaseListener(Button button, GameplayInputType inputType)
        {
            var trigger = button.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
            trigger.triggers.Clear();

            var pressEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pressEntry.callback.AddListener(_ => _gameplayUIService.Press(inputType));
            trigger.triggers.Add(pressEntry);

            var releaseEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            releaseEntry.callback.AddListener(_ => _gameplayUIService.Release(inputType));
            trigger.triggers.Add(releaseEntry);
        }

        private void OnDisable()
        {
            _jumpButton.onClick.RemoveAllListeners();
            _shootButton.onClick.RemoveAllListeners();
            _leftButton.onClick.RemoveAllListeners();
            _rightButton.onClick.RemoveAllListeners();
            _pauseButton.onClick.RemoveAllListeners();
        }
    }
}
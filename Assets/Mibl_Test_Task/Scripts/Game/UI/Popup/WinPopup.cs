using System;
using Mibl_Test_Task.Scripts.Services.GameStateService;
using Services.PersistenceProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.Popup
{
    public class WinPopup: MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _quitButton;
        private ILevelStateService _levelStateService;

        [Inject]
        public void Construct(ILevelStateService levelStateService)
        {
            _levelStateService = levelStateService;
        }

        public void Initialize()
        {
            _continueButton.onClick.AddListener(_levelStateService.Restart);
            _quitButton.onClick.AddListener(_levelStateService.Restart);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
        }
    }
}
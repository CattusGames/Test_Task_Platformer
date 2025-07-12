using System;
using Mibl_Test_Task.Scripts.Services.GameStateService;
using Services.PersistenceProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.Popup
{
    public class PausePopup: MonoBehaviour
    {
        [SerializeField] private Text _levelText;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _quitButton;
        private IPersistenceProgressService _persistenceProgressService;
        private ILevelStateService _levelStateService;

        [Inject]
        public void Construct(IPersistenceProgressService persistenceProgressService, ILevelStateService levelStateService)
        {
            _levelStateService = levelStateService;
            _persistenceProgressService = persistenceProgressService;
        }

        public void Initialize()
        {
            _levelText.text = "Level " + (_persistenceProgressService.Player.CurrentLevelId + 1);
            
            _exitButton.onClick.AddListener(_levelStateService.Resume);
            _continueButton.onClick.AddListener(_levelStateService.Resume);
            _quitButton.onClick.AddListener(_levelStateService.Quit);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveAllListeners();
            _continueButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
        }
    }
}
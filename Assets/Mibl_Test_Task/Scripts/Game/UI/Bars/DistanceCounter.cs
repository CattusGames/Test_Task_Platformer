using System;
using System.Collections;
using Mibl_Test_Task.Scripts.Services.GameplayUIService;
using Mibl_Test_Task.Scripts.Services.WinCondition;
using Mibl_Test_Task.Scripts.StaticData;
using Services.Factories.GameFactory;
using Services.Level;
using Services.PersistenceProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.Bars
{
    public class DistanceCounter: MonoBehaviour
    {
        [SerializeField] private Text _text;
        private IGameFactory _gameFactory;
        private ILevelDescriptionService _levelDescriptionService;
        private IPersistenceProgressService _persistenceProgressService;

        [Inject]
        public void Construct(IGameFactory gameFactory, 
            ILevelDescriptionService levelDescriptionService, 
            IPersistenceProgressService persistenceProgressService,
            IWinConditionService winConditionService)
        {
            _persistenceProgressService = persistenceProgressService;
            _levelDescriptionService = levelDescriptionService;
            _gameFactory = gameFactory;
        }

        public void Initialize()
        {
            if (!_levelDescriptionService
                    .ForLevel(_persistenceProgressService.Player.CurrentLevelId)
                    .FinishConditions.HasFlag(FinishConditions.Distance))
            {
                gameObject.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            int distanse = (int)Mathf.Abs(
                _levelDescriptionService.ForLevel(_persistenceProgressService.Player.CurrentLevelId).StartDistance - 
                _gameFactory.Player.gameObject.transform.position.x
            );
                    
            _text.text = distanse + "/" + (int)_levelDescriptionService.ForLevel(_persistenceProgressService.Player.CurrentLevelId).LevelLength;
        }
    }
}
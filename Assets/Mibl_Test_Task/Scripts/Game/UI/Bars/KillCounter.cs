using System;
using Mibl_Test_Task.Scripts.Services.GameplayUIService;
using Mibl_Test_Task.Scripts.StaticData;
using Services.Factories.GameFactory;
using Services.Level;
using Services.PersistenceProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.Bars
{
    public class KillCounter: MonoBehaviour
    {
        [SerializeField] private Text _text;
        private IGameFactory _gameFactory;
        private ILevelDescriptionService _levelDescriptionService;
        private IPersistenceProgressService _persistenceProgressService;

        [Inject]
        public void Construct(IGameFactory gameFactory, ILevelDescriptionService levelDescriptionService, IPersistenceProgressService persistenceProgressService)
        {
            _persistenceProgressService = persistenceProgressService;
            _levelDescriptionService = levelDescriptionService;
            _gameFactory = gameFactory;
        }

        public void Initialize()
        {
            if (!_levelDescriptionService
                    .ForLevel(_persistenceProgressService.Player.CurrentLevelId)
                    .FinishConditions.HasFlag(FinishConditions.KillCount))
            {
                gameObject.SetActive(false);
            }
            else
            {
                _persistenceProgressService.Player.OnKillCountIncreased += ChangeValue;
                ChangeValue();
            }
            
        }

        private void ChangeValue()
        {
            _text.text = _persistenceProgressService.Player.EnemyKillCount + "/" + _levelDescriptionService.ForLevel(_persistenceProgressService.Player.CurrentLevelId)._killTarget;
        }

        private void OnDisable()
        {
            _persistenceProgressService.Player.OnKillCountIncreased -= ChangeValue;
        }
    }
}
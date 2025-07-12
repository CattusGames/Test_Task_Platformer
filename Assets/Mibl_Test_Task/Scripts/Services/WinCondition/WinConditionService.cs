using System;
using System.Collections.Generic;
using Mibl_Test_Task.Scripts.StaticData;
using Services.Factories.GameFactory;
using Services.Level;
using Services.PersistenceProgress;
using UnityEngine;

namespace Mibl_Test_Task.Scripts.Services.WinCondition
{
    public class WinConditionService: IWinConditionService
    {
        private readonly IPersistenceProgressService _persistenceProgressService;
        private readonly ILevelDescriptionService _levelDescriptionService;

        private FinishConditions _currentConditions;
        private Dictionary<FinishConditions, Func<bool>> _conditionChecks = new Dictionary<FinishConditions, Func<bool>>();
        private IGameFactory _gameFactory;
        
        public bool IsWon { get; private set; }
        public void SetWin(bool b)
        {
            IsWon = b;
        }

        public WinConditionService(IPersistenceProgressService persistenceProgressService, ILevelDescriptionService levelDescriptionService, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _levelDescriptionService = levelDescriptionService;
            _persistenceProgressService = persistenceProgressService;
        }

        public void WarmUp()
        {
            IsWon = false;
            
            _currentConditions = _levelDescriptionService.ForLevel(_persistenceProgressService.Player.CurrentLevelId).FinishConditions;

            _conditionChecks = new Dictionary<FinishConditions, Func<bool>>
            {
                { FinishConditions.KillCount, CheckKillCountCondition },
                { FinishConditions.Distance, CheckDistanceCondition }
            };
        }
        
        public bool AreWinConditionsMet()
        {
            foreach (var pair in _conditionChecks)
            {
                if (_currentConditions.HasFlag(pair.Key))
                {
                    if (!pair.Value.Invoke())
                        return false;
                }
            }
            
            IsWon = true;
            return true;
        }

        private bool CheckKillCountCondition()
        {
            return _persistenceProgressService.Player.EnemyKillCount >= _levelDescriptionService.ForLevel(_persistenceProgressService.Player.CurrentLevelId)._killTarget;
        }

        private bool CheckDistanceCondition()
        {
            var levelData = _levelDescriptionService.ForLevel(_persistenceProgressService.Player.CurrentLevelId);
    
            if (_gameFactory.Player == null)
            {
                Debug.LogWarning("Player not yet spawned.");
                return false;
            }

            float distance = Mathf.Abs(levelData.StartDistance - _gameFactory.Player.gameObject.transform.position.x);
            return distance >= levelData.LevelLength;
        }

    }
}
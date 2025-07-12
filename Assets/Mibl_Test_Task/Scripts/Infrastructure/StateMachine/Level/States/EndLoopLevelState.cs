using Mibl_Test_Task.Scripts.Services.WinCondition;
using Services.PersistenceProgress;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.StateMachine.Level.States
{
    public abstract partial class LevelStates
    {
        public class End : IState, ILevelState
        {
            private IWinConditionService _winConditionService;
            private IPersistenceProgressService _persistenceProgress;

            public End(IWinConditionService winConditionService, IPersistenceProgressService persistenceProgress)
            {
                _persistenceProgress = persistenceProgress;
                _winConditionService = winConditionService;
            } 
            public void Exit()
            {
                Time.timeScale = 1f;
            }

            public void Enter()
            {
                Time.timeScale = 0f;

                if (_winConditionService.IsWon)
                {
                    _persistenceProgress.Player.Reset();
                    _persistenceProgress.Player.NextLevelId = _persistenceProgress.Player.CurrentLevelId + 1;
                }
                else
                {
                    _persistenceProgress.Player.Reset();
                }
            }
        }
    }
}
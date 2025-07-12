using Mibl_Test_Task.Scripts.Services.GameStateService;
using Mibl_Test_Task.Scripts.Services.WinCondition;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachine.Level.States
{
    public abstract partial class LevelStates
    {
        public class LevelLoop: IState, ILevelState, ITickable
        {
            private IWinConditionService _winConditionService;
            private ILevelStateService _levelStateService;

            private bool _entered;

            public LevelLoop(IWinConditionService winConditionService, ILevelStateService levelStateService)
            {
                _levelStateService = levelStateService;
                _winConditionService = winConditionService;
            }
            
            public void Exit()
            {
                _entered = false;
            }

            public void Enter()
            {
                _entered = true;
            }

            public void Tick()
            {
                if(!_entered)
                    return;
                
                if (_winConditionService.AreWinConditionsMet() || Input.GetKeyDown(KeyCode.W))
                {
                    _winConditionService.SetWin(true);
                    _levelStateService.Win();
                }
            }
        }
    }
}
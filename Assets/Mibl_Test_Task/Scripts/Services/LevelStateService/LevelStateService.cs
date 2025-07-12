using System;
using Game.UI.Popup;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Level;
using Infrastructure.StateMachine.Level.States;
using Mibl_Test_Task.Scripts.Services.GameplayUIService;
using Services.Window;
using Window;

namespace Mibl_Test_Task.Scripts.Services.GameStateService
{
    public class LevelStateService: ILevelStateService, IDisposable
    {
        private IStateMachine<ILevelState> _levelStateMachine;
        private IWindowService _windowService;
        private IGameplayUIService _gameplayUIService;

        private bool _isLose;
        private bool _isWin;

        public LevelStateService(IStateMachine<ILevelState> levelStateMachine, IWindowService windowService, IGameplayUIService gameplayUIService)
        {
            _gameplayUIService = gameplayUIService;
            _windowService = windowService;
            _levelStateMachine = levelStateMachine;

            _gameplayUIService.OnInputUp += HandleInputTriggered;
        }

        private void HandleInputTriggered(GameplayInputType obj)
        {
            if (obj == GameplayInputType.Pause)
                Pause();
        }

        public async void Lose()
        {
            if (_isLose)
                return;
            
            _isLose = true;
            var losePopup = await _windowService.ShowPopup<LosePopup>(WindowTypeId.LosePopup);
            losePopup.Initialize();
            _levelStateMachine.Enter<LevelStates.End>();
        }

        public async void Win()
        {
            if (_isWin)
                return;
            
            _isWin = true;
            var winPopup = await _windowService.ShowPopup<WinPopup>(WindowTypeId.WinPopup);
            winPopup.Initialize();
            _levelStateMachine.Enter<LevelStates.End>();
        }

        public async void Pause()
        {
            var pausePopup = await _windowService.ShowPopup<PausePopup>(WindowTypeId.PausePopup);
            pausePopup.Initialize();
            _levelStateMachine.Enter<LevelStates.Pause>();
        }

        public async void Resume()
        {
            await _windowService.CloseWindow(WindowTypeId.PausePopup);
            _levelStateMachine.Enter<LevelStates.LevelLoop>();
        }

        public async void Restart()
        {
            _levelStateMachine.Enter<LevelStates.CleanUpLevelState>();
            CleanUp();
        }

        public async void Quit()
        {
            _levelStateMachine.Enter<LevelStates.CleanUpLevelState>();
            CleanUp();
        }

        public void CleanUp()
        {
            _isLose = false;
            _isWin = false;
        }

        public void Dispose()
        {
            _gameplayUIService.OnInputUp -= HandleInputTriggered;
        }
    }
}
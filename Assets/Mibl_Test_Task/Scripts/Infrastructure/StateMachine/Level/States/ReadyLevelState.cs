using Game.UI;
using Services.Factories.GameFactory;
using Services.InputHandler;
using Services.Window;
using UnityEngine;
using Window;
using Zenject.SpaceFighter;

namespace Infrastructure.StateMachine.Level.States
{
    public abstract partial class LevelStates
    {
        public class Ready : IState, ILevelState
        {
            private IWindowService _windowService;
            private IStateMachine<ILevelState> _stateMachine;

            public Ready(IWindowService windowService, IStateMachine<ILevelState> stateMachine)
            {
                _stateMachine = stateMachine;
                _windowService = windowService;
            }
            public void Exit()
            {
            }

            public void Enter()
            {
                _windowService.Windows[WindowTypeId.Main].GetComponentInChildren<MainWindow>().Initialize();
                
                _stateMachine.Enter<LevelStates.LevelLoop>();
            }
        }
    }
}
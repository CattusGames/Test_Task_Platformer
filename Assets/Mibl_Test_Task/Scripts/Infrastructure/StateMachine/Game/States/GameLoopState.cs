using Infrastructure.StateMachine.Level;
using Infrastructure.StateMachine.Level.States;

namespace Infrastructure.StateMachine.Game.States
{
    public abstract partial class GameStates
    {
        public class GameLoop : IState, IGameState
        {
            private IStateMachine<ILevelState> _levelStateMachine;

            public GameLoop(IStateMachine<ILevelState> levelStateMachine)
            {
                _levelStateMachine = levelStateMachine;
            }

            public void Exit()
            {
                _levelStateMachine.Enter<LevelStates.Idle>();
            }

            public void Enter()
            {
            }
        }
    }
}
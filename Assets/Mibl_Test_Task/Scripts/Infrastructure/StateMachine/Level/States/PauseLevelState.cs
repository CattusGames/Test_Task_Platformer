using UnityEngine;

namespace Infrastructure.StateMachine.Level.States
{
    public abstract partial class LevelStates
    {
        public class Pause : IState, ILevelState
        {
            public void Enter()
            {
                Time.timeScale = 0f;
            }
            public void Exit()
            {
                Time.timeScale = 1f;
            }
        }
    }
}
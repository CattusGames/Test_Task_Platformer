using UnityEngine;

namespace Infrastructure.StateMachine.Level.States
{
    public abstract partial class LevelStates
    {
        public class Idle : IState, ILevelState
        {
            public void Exit()
            {
            }

            public void Enter()
            {
            }
        }
    }
}
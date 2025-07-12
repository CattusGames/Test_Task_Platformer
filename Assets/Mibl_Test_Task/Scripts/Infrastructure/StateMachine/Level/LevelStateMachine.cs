using Zenject;

namespace Infrastructure.StateMachine.Level
{
    public class LevelStateMachine : StateMachine<ILevelState>, ITickable
    {
        public LevelStateMachine(LevelStateFactory stateFactory) : base(stateFactory)
        {
        }

        public void Tick()
        {
            Update();
        }
    }
}
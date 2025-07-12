using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Level.States;
using Zenject;

namespace Infrastructure.StateMachine.Level
{
    public class LevelStateFactory : StateFactory
    {
        public LevelStateFactory(DiContainer container) : base(container)
        {
        }

        protected override Dictionary<Type, Func<IExitable>> BuildStatesRegister(DiContainer container)
        {
            return new Dictionary<Type, Func<IExitable>>()
            {
                [typeof(LevelStates.Ready)] = container.Resolve<LevelStates.Ready>,
                [typeof(LevelStates.LoadLevel)] = container.Resolve<LevelStates.LoadLevel>,
                [typeof(LevelStates.StartLevel)] = container.Resolve<LevelStates.StartLevel>,
                [typeof(LevelStates.LevelLoop)] = container.Resolve<LevelStates.LevelLoop>,
                [typeof(LevelStates.CleanUpLevelState)] = container.Resolve<LevelStates.CleanUpLevelState>,
                [typeof(LevelStates.Pause)] = container.Resolve<LevelStates.Pause>,
                [typeof(LevelStates.End)] = container.Resolve<LevelStates.End>,
                [typeof(LevelStates.Idle)] = container.Resolve<LevelStates.Idle>
            };
        }
    }
}
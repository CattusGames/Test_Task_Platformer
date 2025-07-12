using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Game.States;
using Zenject;

namespace Infrastructure.StateMachine.Game
{
    public class GameStateFactory : StateFactory
    {
        public GameStateFactory(DiContainer container) : base(container)
        {
        }

        protected override Dictionary<Type, Func<IExitable>> BuildStatesRegister(DiContainer container)
        {
            return new Dictionary<Type, Func<IExitable>>()
            {
                [typeof(GameStates.Bootstrap)] = container.Resolve<GameStates.Bootstrap>,
                [typeof(GameStates.LoadGame)] = container.Resolve<GameStates.LoadGame>,
                [typeof(GameStates.LoadProgress)] = container.Resolve<GameStates.LoadProgress>,
                [typeof(GameStates.GameLoop)] = container.Resolve<GameStates.GameLoop>,
            };
        }

        
    }
}
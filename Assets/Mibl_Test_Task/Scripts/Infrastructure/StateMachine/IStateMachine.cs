using System;
using System.Threading.Tasks;

namespace Infrastructure.StateMachine
{
    public interface IStateMachine<TBaseState>
    {
        Type ActiveStateType { get; }
        TState Enter<TState>() where TState : class, TBaseState, IState;
        TState Enter<TState, TPayload>(TPayload payload) where TState : class, TBaseState, IPayloadedState<TPayload>;
        bool Back();
        Task<TState> EnterAsync<TState, TPayload>(TPayload payload) where TState : class, TBaseState, IPayloadedStateAsync<TPayload>;
    }
}
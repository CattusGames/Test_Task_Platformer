using System.Threading.Tasks;

namespace Infrastructure.StateMachine
{
    public interface IPayloadedState<TPayload> : IExitable
    {
        void Enter(TPayload payload);
    }
    
    public interface IPayloadedStateAsync<TPayload> : IPayloadedState<TPayload>
    {
        Task EnterAsync(TPayload payload);
    }
}
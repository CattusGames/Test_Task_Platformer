namespace Infrastructure.StateMachine
{
    public interface IState : IExitable
    {
        void Enter();
    }
}
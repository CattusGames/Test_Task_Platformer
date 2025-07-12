using Services.StaticData;
using StaticData;

namespace Infrastructure.StateMachine.Game.States
{
    public abstract partial class GameStates
    {
        public class Bootstrap : IState, IGameState
        {
            private readonly IStateMachine<IGameState> _stateMachine;
            private readonly ISceneLoader _sceneLoader;
            private IStaticDataService _staticData;
            private GameStaticData _gameStaticData;
            private ILoadingCurtain _loadingCurtain;


            public Bootstrap(IStateMachine<IGameState> stateMachine, ISceneLoader sceneLoader, IStaticDataService staticData, ILoadingCurtain loadingCurtain)
            {
                _loadingCurtain = loadingCurtain;
                _staticData = staticData;
                _stateMachine = stateMachine;
                _sceneLoader = sceneLoader;
            }

            public void Enter()
            {
                _loadingCurtain.ShowFirst();
                _loadingCurtain.ShowProgress(0.05f);
                _gameStaticData = _staticData.GameConfig();
                
                _sceneLoader.Load(_gameStaticData.InitialScene, OnLevelLoad, _loadingCurtain);
            }

            private void OnLevelLoad() => _stateMachine.Enter<LoadProgress>();

            public void Exit()
            {
                
            }
        }
    }
}
using System.Threading.Tasks;
using Infrastructure.StateMachine.Game;
using Infrastructure.StateMachine.Game.States;
using Services.Factories.GameFactory;
using Services.PersistenceProgress;
using Services.SaveLoad;
using Services.Window;
using UnityEngine;

namespace Infrastructure.StateMachine.Level.States
{
    public abstract partial class LevelStates
    {
        public class CleanUpLevelState: IState, ILevelState
        {
            private IStateMachine<IGameState> _gameStateMachine;
            private IStateMachine<ILevelState> _levelStateMachine;
            private ICoroutineRunner _coroutineRunner;
            private ILoadingCurtain _loadingCurtain;
            private IPersistenceProgressService _persistenceProgressService;
            private IGameFactory _gameFactory;
            private IWindowService _windowService;
            private ISaveLoadService _saveLoadService;

            public CleanUpLevelState(IStateMachine<IGameState> gameStateMachine, 
                IStateMachine<ILevelState> levelStateMachine,
                ILoadingCurtain loadingCurtain,
                IPersistenceProgressService persistenceProgressService,
                IGameFactory gameFactory,
                IWindowService windowService,
                ISaveLoadService saveLoadService)
            {
                _saveLoadService = saveLoadService;
                _windowService = windowService;
                _gameFactory = gameFactory;
                _persistenceProgressService = persistenceProgressService;
                _loadingCurtain = loadingCurtain;
                _levelStateMachine = levelStateMachine;
                _gameStateMachine = gameStateMachine;
            }
            public void Exit()
            {
            }

            public async void Enter()
            {
                _loadingCurtain.Show();
                
                await ResetGameWorld();

                if (_persistenceProgressService.Player.CurrentLevelId != _persistenceProgressService.Player.NextLevelId)
                {
                    _persistenceProgressService.Player.CurrentLevelId = _persistenceProgressService.Player.NextLevelId;
                    _saveLoadService.SaveProgress();
                    Debug.Log(_saveLoadService.LoadProgress().CurrentLevelId);
                    _levelStateMachine.Enter<Idle>();
                    _gameStateMachine.Enter<GameStates.LoadGame, int>(_persistenceProgressService.Player.NextLevelId);
                }
                else
                {
                    _gameStateMachine.Enter<GameStates.GameLoop>();
                    await _levelStateMachine.EnterAsync<LevelStates.LoadLevel, int>(_persistenceProgressService.Player.CurrentLevelId);
                }
            }

            private async Task ResetGameWorld()
            {
                await RefreshEnemySpawner();
                DestroyMainActors();
                _windowService.CleanUp();
            }

            private void DestroyMainActors()
            {
                if (_gameFactory.Player != null)
                    GameObject.Destroy(_gameFactory.Player.gameObject);
            }


            private async Task RefreshEnemySpawner()
            {
                _gameFactory.EnemySpawner.CleanUp();
                GameObject.Destroy(_gameFactory.EnemySpawner.gameObject);
                await _gameFactory.CreateEnemySpawner();
            }
        }
    }
}
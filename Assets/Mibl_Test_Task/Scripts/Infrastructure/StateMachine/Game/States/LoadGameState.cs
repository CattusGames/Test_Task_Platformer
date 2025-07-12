using System.Collections.Generic;
using System.Threading.Tasks;
using Game.UI;
using Infrastructure.StateMachine.Level;
using Infrastructure.StateMachine.Level.States;
using Services.AssetProvider;
using Services.Factories.GameFactory;
using Services.Factories.UIFactory;
using Services.Level;
using Services.PersistenceProgress;
using Services.Window;
using StaticData;
using UnityEngine;
using UnityEngine.UI;
using Window;

namespace Infrastructure.StateMachine.Game.States
{
    public abstract partial class GameStates
    {
        public class LoadGame : IPayloadedState<int>, IGameState
        {
            private IStateMachine<IGameState> _gameStateMachine;
            private IStateMachine<ILevelState> _levelStateMachine;
            private IGameFactory _gameFactory;
            private ISceneLoader _sceneLoader;
            private IAssetProvider _assetProvider;
            private ILoadingCurtain _loadingCurtain;
            private IPersistenceProgressService _progress;
            private ILevelDescriptionService _levelDescriptionService;
            private IUIFactory _uiFactory;
            private IWindowService _windowService;

            public LoadGame(IStateMachine<IGameState> gameStateMachine,
                IStateMachine<ILevelState> levelStateMachine,
                IGameFactory gameFactory,
                IUIFactory uiFactory,
                ISceneLoader sceneLoader, 
                IAssetProvider assetProvider, 
                ILoadingCurtain loadingCurtain, 
                IPersistenceProgressService progress,
                ILevelDescriptionService levelDescriptionService,
                IWindowService windowService)
            {
                _windowService = windowService;
                _levelStateMachine = levelStateMachine;
                _uiFactory = uiFactory;
                _levelDescriptionService = levelDescriptionService;
                _progress = progress;
                _loadingCurtain = loadingCurtain;
                _gameStateMachine = gameStateMachine;
                _assetProvider = assetProvider;
                _sceneLoader = sceneLoader;
                _gameFactory = gameFactory;
            }
            public async void Enter(int level)
            {
                ShowCurtain();
                _assetProvider.CleanUp();
                ScriptableAccident descriptor = _levelDescriptionService.ForLevel(level);
                
                await _gameFactory.WarmUp();
                
                _sceneLoader.LoadForce(descriptor.SceneName, () => OnLevelLoad(level), _loadingCurtain);
            }
            
            private void ShowCurtain()
            {
                if (_progress.Player.StartGameCount == 0)
                {
                    _loadingCurtain.ShowFirst();
                    _loadingCurtain.ShowProgress(0.15f);
                }
                else
                {
                    _loadingCurtain.Show();
                }
            }

            private async void OnLevelLoad(int levelId)
            {
                _progress.Player.CurrentLevelId = levelId;
                ScriptableAccident accident = _levelDescriptionService.ForLevel(levelId);
                
                await InitGameWorld(accident);
                _gameStateMachine.Enter<GameLoop>();
                await EnterLevelState(levelId);
            }

            private async Task EnterLevelState(int levelId) =>  await _levelStateMachine.EnterAsync<LevelStates.LoadLevel,int>(levelId);

            private async Task InitGameWorld(ScriptableAccident accident)
            {
                List<Task> tasks = new List<Task>
                {
                    _uiFactory.CreateUIRoot(),
                    _gameFactory.CreateEnemySpawner()
                };
                await Task.WhenAll(tasks);
            }

            public void Exit()
            {
            }
        }

    }
}
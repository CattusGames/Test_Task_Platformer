using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Level;
using Infrastructure.StateMachine.Level.States;
using Mibl_Test_Task.Scripts.Services.GameplayUIService;
using Mibl_Test_Task.Scripts.Services.GameStateService;
using Mibl_Test_Task.Scripts.Services.WinCondition;
using Services.AssetProvider;
using Services.Factories.GameFactory;
using Services.Factories.UIFactory;
using Services.InputHandler;
using Services.Level;
using Services.LevelContext;
using Services.PersistenceProgress;
using Services.SaveLoad;
using Services.StaticData;
using Services.Window;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private LoadingCurtain _curtain;
        
        public override void InstallBindings()
        {
            Debug.Log("InstallBindings");
            
            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this);
            
            BindMonoServices();
            BindSceneLoader();
            BindServices();
            BindGameStateMachine();
            BindGameStates();
            BindLevelStateMachine();
            BindLevelStates();
        }

        private void BindMonoServices()
        {
            Container.Bind<ICoroutineRunner>().FromMethod(() => Container.InstantiatePrefabForComponent<ICoroutineRunner>(_coroutineRunner)).AsSingle();
            Container.Bind<ILoadingCurtain>().FromMethod(() => Container.InstantiatePrefabForComponent<ILoadingCurtain>(_curtain)).AsSingle();
        }
        
        private void BindGameStateMachine()
        {
            Container.Bind<GameStateFactory>().AsSingle();
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
        }

        private void BindGameStates()
        {
            Container.Bind<GameStates.Bootstrap>().AsSingle();
            Container.Bind<GameStates.LoadGame>().AsSingle();
            Container.Bind<GameStates.LoadProgress>().AsSingle();
            Container.Bind<GameStates.GameLoop>().AsSingle();
        }
        
        private void BindLevelStates()
        {
            Container.Bind<LevelStateFactory>().To<LevelStateFactory>().AsSingle();
            Container.BindInterfacesTo<LevelStateMachine>().AsSingle();
        }

        private void BindLevelStateMachine()
        {
            Container.Bind<LevelStates.Ready>().AsSingle();
            Container.Bind<LevelStates.LoadLevel>().AsSingle();
            Container.Bind<LevelStates.StartLevel>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelStates.LevelLoop>().AsSingle();
            Container.Bind<LevelStates.CleanUpLevelState>().AsSingle();
            Container.Bind<LevelStates.Pause>().AsSingle();
            Container.Bind<LevelStates.End>().AsSingle();
            Container.Bind<LevelStates.Idle>().AsSingle();
        }

        private void BindSceneLoader()
        { 
            Container.BindInterfacesTo<SceneLoader>().AsSingle();
        }
        
        private void BindServices()
        {
            BindStaticDataService();
            BindAssetProvider();
            
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.Bind<IPersistenceProgressService>().To<PersistenceProgressService>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<ILevelDescriptionService>().To<LevelDescriptionService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<ILevelStateService>().To<LevelStateService>().AsSingle();
            Container.Bind<IWinConditionService>().To<WinConditionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayUIService>().AsSingle();
            Container.BindInterfacesTo<TouchInput>().AsSingle();
            Container.BindInterfacesTo<SingletonLevelContext>().AsSingle().NonLazy();
        }
        
        private void BindStaticDataService()
        {
            IStaticDataService staticDataService = Container.Instantiate<StaticDataService>();
            staticDataService.LoadData();
            Container.Bind<IStaticDataService>().FromInstance(staticDataService).AsSingle();
        }

        private void BindAssetProvider()
        {
            Container.Bind<IAssetProvider>().FromMethod(() => Container.Instantiate<AssetProviderService>()).AsSingle();
        }

        public void Initialize()
        {
            Debug.Log("Initializing bootstrap installer");

            BootstrapGame();
        }

        private void BootstrapGame() => 
            Container.Resolve<IStateMachine<IGameState>>().Enter<GameStates.Bootstrap>();
    }
}

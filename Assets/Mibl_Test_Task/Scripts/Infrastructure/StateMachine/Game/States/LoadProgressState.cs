using Services.PersistenceProgress;
using Services.PersistenceProgress.Player;
using Services.SaveLoad;

namespace Infrastructure.StateMachine.Game.States
{
    public abstract partial class GameStates
    {
        public class LoadProgress : IState, IGameState
        {
            private IStateMachine<IGameState> _stateMachine;
            private IPersistenceProgressService _persistenceProgressService;
            private ILoadingCurtain _loadingCurtain;
            private ISaveLoadService _saveLoadService;

            public LoadProgress(IStateMachine<IGameState> stateMachine, 
                IPersistenceProgressService persistenceProgressService,
                ILoadingCurtain loadingCurtain, 
                ISaveLoadService saveLoadService)
            {
                _saveLoadService = saveLoadService;
                _loadingCurtain = loadingCurtain;
                _persistenceProgressService = persistenceProgressService;
                _stateMachine = stateMachine;
            }
            
            public void Enter()
            {
                _loadingCurtain.ShowFirst();
                _loadingCurtain.ShowProgress(0.10f);
                
                LoadOrCreatePlayerData();
                
                int level = _persistenceProgressService.Player.CurrentLevelId;
                _stateMachine.Enter<LoadGame, int>(level);
            }
            
            public void Exit()
            {
            }
            
            private PlayerData LoadOrCreatePlayerData() =>
                _persistenceProgressService.Player =
                    _saveLoadService.LoadProgress()
                    ?? CreateNew();
            
            private PlayerData CreateNew()
            {
                var data = new PlayerData();
                
                return data;
            }
        }
    }
}
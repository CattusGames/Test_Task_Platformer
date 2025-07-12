using System.Threading.Tasks;
using Game.UI;
using Infrastructure.LevelContext;
using Mibl_Test_Task.Scripts.Game;
using Services.Factories.GameFactory;
using Services.Level;
using Services.Window;
using StaticData;
using UnityEngine;
using Window;

namespace Infrastructure.StateMachine.Level.States
{
    public abstract partial class LevelStates
    {
        public class LoadLevel : IPayloadedStateAsync<int>, ILevelState
        {
            private ILevelDescriptionService _levelDescriptionService;
            private IGameFactory _gameFactory;
            private ILevelContext _levelContext;
            private IStateMachine<ILevelState> _levelStateMachine;
            private ILoadingCurtain _loadingCurtain;

            public LoadLevel(IStateMachine<ILevelState> levelStateMachine,
                ILevelDescriptionService levelDescriptionService, 
                IGameFactory gameFactory,
                ILevelContext levelContext,
                ILoadingCurtain loadingCurtain)
            {
                _loadingCurtain = loadingCurtain;
                _levelStateMachine = levelStateMachine;
                _levelContext = levelContext;
                _gameFactory = gameFactory;
                _levelDescriptionService = levelDescriptionService;
            }
            
            public void Enter(int payload)
            {
            }

            public async Task EnterAsync(int payload)
            {
                await ExecuteState(payload);
            }

            private async Task ExecuteState(int payload)
            {
                ScriptableAccident descriptor = _levelDescriptionService.ForLevel(payload);
                
                _loadingCurtain.ShowProgress(0.6f);
                
                _gameFactory.CreateRootParents();
                await _gameFactory.CreatePlayer(new Vector3(0,0,descriptor.StartDistance));
                await _gameFactory.EnemySpawner.GetComponent<EnemySpawner>().Initialize();
                InitCamera();
                BootsrtapEnemySpawner(descriptor);
                await _levelStateMachine.EnterAsync<LevelStates.StartLevel, int>(payload);
            }

            private void InitCamera()
            {
                _levelContext.CameraMovement.Initialize();
            }

            private void BootsrtapEnemySpawner(ScriptableAccident descriptor)
            {
            }

            public void Exit()
            {
            }
        }
    }
}
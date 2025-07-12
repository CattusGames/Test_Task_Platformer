using System.Threading.Tasks;
using Game.UI;
using Infrastructure.LevelContext;
using Mibl_Test_Task.Scripts.Game.Camera;
using Mibl_Test_Task.Scripts.Services.WinCondition;
using Services.Window;
using UnityEngine;
using Window;

namespace Infrastructure.StateMachine.Level.States
{
    public abstract partial class LevelStates
    {
        public class StartLevel: IPayloadedStateAsync<int>, ILevelState
        {
            private ILevelContext _levelContext;
            private ILoadingCurtain _loadingCurtain;
            private IStateMachine<ILevelState> _levelStateMachine;
            private IWindowService _windowService;
            private IWinConditionService _winConditionService;

            public StartLevel(IStateMachine<ILevelState> levelStateMachine,
                ILevelContext levelContext, 
                ILoadingCurtain loadingCurtain,
                IWindowService windowService,
                IWinConditionService winConditionService)
            {
                _winConditionService = winConditionService;
                _windowService = windowService;
                _levelStateMachine = levelStateMachine;
                _loadingCurtain = loadingCurtain;
                _levelContext = levelContext;
            }

            public void Enter(int payload)
            {
            }

            public async Task EnterAsync(int payload)
            {
                RectTransform windowRect = await _windowService.OpenWindow(WindowTypeId.Home);
                windowRect.GetComponentInChildren<HomeWindow>().Initialize(payload);
                
                _loadingCurtain.ShowProgress(1f);
                _loadingCurtain.Hide();
                
                while (_loadingCurtain.Current != LoadingCurtain.Status.Hided)
                    await Task.Yield();
                
                await windowRect.GetComponentInChildren<HomeWindow>().WaitForStartButtonClickAsync();
                await _windowService.CloseWindow(WindowTypeId.Home);
                await _windowService.OpenWindow(WindowTypeId.Main);

                _winConditionService.WarmUp();
                _levelStateMachine.Enter<LevelStates.Ready>();
            }

            public void Exit()
            {
                Debug.Log("ExitStartLevel State");
            }
        }
    }
}
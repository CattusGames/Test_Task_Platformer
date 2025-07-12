using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


namespace Infrastructure
{ 
    public class SceneLoader : ISceneLoader
    {
        private const float MinDisplayedProgress = 0.2f;
        
        private ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLevelLoad, ILoadingCurtain loadingCurtain)
        {
            _coroutineRunner.StartCoroutine(LoadLevel(name, onLevelLoad, loadingCurtain, true));
        }

        public void LoadForce(string name, Action onLevelLoad, ILoadingCurtain loadingCurtain)
        {
            _coroutineRunner.StartCoroutine(LoadLevel(name, onLevelLoad, loadingCurtain, false));
        }

        private IEnumerator LoadLevel(string name, Action onLevelLoad, ILoadingCurtain loadingCurtain, bool loadForce)
        {
            if (!loadForce && SceneManager.GetActiveScene().name == name)
            {
                onLevelLoad?.Invoke();
                yield break;
            }
            
            AsyncOperationHandle<SceneInstance> asyncOperationHandle = Addressables.LoadSceneAsync(name);
            
            while (!asyncOperationHandle.IsDone)
            {
                float progress = asyncOperationHandle.PercentComplete;
                loadingCurtain?.ShowProgress(Mathf.Max(MinDisplayedProgress,progress));
                yield return null;
            }

            if (!asyncOperationHandle.Result.Scene.IsValid())
            {
                Debug.Log($"scene is not valid!");
                yield break;
            }
            
            onLevelLoad?.Invoke();
        }
    }
}
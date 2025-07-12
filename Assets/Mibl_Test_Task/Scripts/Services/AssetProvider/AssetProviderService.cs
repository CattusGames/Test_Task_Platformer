using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services.AssetProvider
{
    public class AssetProviderService: IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedHandles = new Dictionary<string, AsyncOperationHandle>();
        private readonly Dictionary<string, AsyncOperationHandle> _completedPersistenceHandles = new Dictionary<string, AsyncOperationHandle>();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _persistenceHandles = new Dictionary<string, List<AsyncOperationHandle>>();

        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            string cacheKey = assetReference.AssetGUID + assetReference.SubObjectName;
            if (_completedHandles.TryGetValue(cacheKey, out AsyncOperationHandle completedHandle) ||
                _completedPersistenceHandles.TryGetValue(cacheKey, out completedHandle))
                return completedHandle.Result as T;

            return await RunWithCacheOnComplete(
                Addressables.LoadAssetAsync<T>(assetReference),
                cacheKey: cacheKey);
        }

        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += h =>
            {
                _completedHandles[cacheKey] = h;
            };

            AddTempHandle(cacheKey, handle);
            return await handle.Task;
        }
        public async Task<T> LoadPersistence<T>(string address) where T : class
        {
            if (_completedHandles.TryGetValue(address, out AsyncOperationHandle completedHandle) ||
                _completedPersistenceHandles.TryGetValue(address, out completedHandle))
                return completedHandle.Result as T;
            
            return await RunWithPersistenceCacheOnComplete(
                Addressables.LoadAssetAsync<T>(address),
                cacheKey: address);
        }
        
        private async Task<T> RunWithPersistenceCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += h =>
            {
                _completedPersistenceHandles[cacheKey] = h;
            };

            AddPersistenceHandle(cacheKey, handle);
            return await handle.Task;
        }
        
        private void AddTempHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class =>
            AddHandle(key, handle, _handles);
        
        private void AddPersistenceHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class =>
            AddHandle(key, handle, _persistenceHandles);
        
        private static void AddHandle<T>(string key, AsyncOperationHandle<T> handle, Dictionary<string, List<AsyncOperationHandle>> handleCollection) where T : class
        {
            if (!handleCollection.TryGetValue(key, out List<AsyncOperationHandle> handles))
            {
                handles = new List<AsyncOperationHandle>();
                handleCollection[key] = handles;
            }

            handles.Add(handle);
        }
        
        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> handles in _handles.Values)
            foreach (AsyncOperationHandle handle in handles)
                Addressables.Release(handle);
            
            _handles.Clear();
            _completedHandles.Clear();
        }
    }
}
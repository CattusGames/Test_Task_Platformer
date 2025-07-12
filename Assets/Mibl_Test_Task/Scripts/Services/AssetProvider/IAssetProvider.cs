using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Services.AssetProvider
{
    public interface IAssetProvider
    {
        void CleanUp();
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> LoadPersistence<T>(string address) where T : class;
    }
}
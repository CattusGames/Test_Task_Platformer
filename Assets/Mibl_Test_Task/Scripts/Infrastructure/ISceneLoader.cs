using System;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ISceneLoader
    {
        void Load(string name, Action onLevelLoad, ILoadingCurtain loadingCurtain);
        void LoadForce(string descriptorSceneName, Action action, ILoadingCurtain loadingCurtain);
    }
}
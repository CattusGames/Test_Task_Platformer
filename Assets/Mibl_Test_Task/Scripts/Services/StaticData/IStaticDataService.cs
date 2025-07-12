using StaticData;
using Window;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        void LoadData();
        GameStaticData GameConfig();
        WindowConfig ForWindow(WindowTypeId windowTypeId);
        int LevelDescriptorsCount { get; }
        ScriptableAccident ForLevel(int level);
    }
}
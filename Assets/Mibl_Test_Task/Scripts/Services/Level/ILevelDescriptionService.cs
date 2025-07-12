using StaticData;

namespace Services.Level
{
    public interface ILevelDescriptionService
    {
        ScriptableAccident ForLevel(int level);
        int GetProgressOnLevel(int level, float passedDistance);
        int TotalLevel();
    }
}
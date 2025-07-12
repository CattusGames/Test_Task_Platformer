using System.Linq;
using Services.StaticData;
using StaticData;
using UnityEngine;

namespace Services.Level
{
    public class LevelDescriptionService : ILevelDescriptionService
    {
        private readonly IStaticDataService _staticData;

        public LevelDescriptionService(IStaticDataService staticData)
        {
            _staticData = staticData;
        }
        
        public ScriptableAccident ForLevel(int level)
        {
            int totalLevels = _staticData.LevelDescriptorsCount;

            if(level < totalLevels) 
            { 
                return _staticData.ForLevel(level);
            }
            else
            {
                ScriptableAccident[] repeatable = Enumerable.Range(0, totalLevels)
                    .Select(index => _staticData.ForLevel(index))
                    .ToArray();

                return repeatable[(level - totalLevels) % repeatable.Length];
            }
        }

        public int GetProgressOnLevel(int level, float passedDistance) => 
            (int) Mathf.Clamp((passedDistance / ForLevel(level).LevelLength) * 100,0, 100);

        public int TotalLevel() => _staticData.LevelDescriptorsCount;
    }
}
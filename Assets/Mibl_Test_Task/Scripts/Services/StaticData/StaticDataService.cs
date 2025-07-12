using System;
using System.Collections.Generic;
using System.Linq;
using StaticData;
using UnityEngine;
using Window;

namespace Services.StaticData
{
    public class Paths
    {
        public class StaticData
        {
            public const string GameConfig = "StaticData/Balance/GameConfig";
            public const string Windows = "StaticData/Windows/Windows";
            public const string Levels = "StaticData/Levels";
        }
    }
    
    public class StaticDataService: IStaticDataService
    {
        private GameStaticData _gameStaticData;
        private Dictionary<WindowTypeId, WindowConfig> _windowConfigs = new Dictionary<WindowTypeId, WindowConfig>();
        private Dictionary<int,ScriptableAccident> _levels = new Dictionary<int, ScriptableAccident>();
        
        public int LevelDescriptorsCount => _levels.Count;

        public void LoadData()
        {
            _gameStaticData = Resources.Load<GameStaticData>(Paths.StaticData.GameConfig);
            
            var windowStaticData = Resources.Load<WindowStaticData>(Paths.StaticData.Windows);
            _windowConfigs = windowStaticData.Configs.ToDictionary(x => x.WindowTypeId, x => x);
            
            _levels = Resources
                .LoadAll<ScriptableAccident>(Paths.StaticData.Levels)
                .ToList()
                .ToDictionary(x => x.ID, x => x);
        }

        public GameStaticData GameConfig() => _gameStaticData;

        public WindowConfig ForWindow(WindowTypeId windowTypeId) => _windowConfigs[windowTypeId];

        public ScriptableAccident ForLevel(int level)
        {
            if (level < 0)
                throw new InvalidOperationException($"Level index cannot be negative");

            return _levels[level];
        }
    }
}
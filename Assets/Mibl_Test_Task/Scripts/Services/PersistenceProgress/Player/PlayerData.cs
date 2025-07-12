using System;

namespace Services.PersistenceProgress.Player
{
    [Serializable]
    public class PlayerData
    {
        public int CurrentLevelId;
        public int NextLevelId;
        public int StartGameCount;
        public int EnemyKillCount;
        
        public event Action OnKillCountIncreased;

        public void IncreaseKillCount()
        {
            EnemyKillCount++; 
            OnKillCountIncreased?.Invoke();
        }

        public void Reset()
        {
            EnemyKillCount = 0;
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Game;
using Infrastructure.LevelContext;
using Services.Factories.GameFactory;
using UnityEngine;
using Zenject;

namespace Mibl_Test_Task.Scripts.Game
{
    public class EnemySpawner: MonoBehaviour
    {
        private ILevelContext _levelContext;
        private IGameFactory _gameFactory;
        
        private List<GameObject> _spawnedEnemies = new List<GameObject>();

        [Inject]
        public void Construct(ILevelContext levelContext, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _levelContext = levelContext;
        }
        public async Task Initialize()
        {
            foreach (EnemySpawnSpot enemySpawnSpot in _levelContext.EnemySpawnSpots)
            {
                var enemy = await _gameFactory.SpawnEnemy(enemySpawnSpot);
                _spawnedEnemies.Add(enemy);
            }
        }

        public void CleanUp()
        {
            foreach (GameObject enemy in _spawnedEnemies)
            {
                Destroy(enemy);
            }
            
            _spawnedEnemies.Clear();
        }
    }
}
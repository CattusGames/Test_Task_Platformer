using System.Threading.Tasks;
using Game;
using Mibl_Test_Task.Scripts.Game;
using Mibl_Test_Task.Scripts.Game.Attack.Projectile;
using UnityEngine;

namespace Services.Factories.GameFactory
{
    public interface IGameFactory
    {
        GameObject Player { get; }
        EnemySpawner EnemySpawner { get; }
        Task<GameObject> CreatePlayer(Vector3 position);
        Task WarmUp();
        Task<EnemySpawner> CreateEnemySpawner();
        Task LoadProjectile(ProjectileType type);
        Projectile GetProjectile(ProjectileType projectileType);
        Task<GameObject> SpawnEnemy(EnemySpawnSpot spawnSpot);
        void CreateRootParents();
    }
}
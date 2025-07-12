using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game;
using Mibl_Test_Task.Scripts.Game;
using Mibl_Test_Task.Scripts.Game.Attack;
using Mibl_Test_Task.Scripts.Game.Attack.Projectile;
using Services.AssetProvider;
using Services.Level;
using Services.PersistenceProgress;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Services.Factories.GameFactory
{
    public class GameFactory: Factory, IGameFactory
    {
        
        private const string PlayerPrefabAddress = "Player";
        private const string EnemySpawnerAddress = "EnemySpawner";
        
        private GameObject _playerPrefab;
        private GameObject _enemySpawnerPrefab;
        
        private readonly IAssetProvider _assetProvider;
        private IPersistenceProgressService _persistenceProgress;
        private ILevelDescriptionService _descriptionService;
        private Transform _enemiesRoot;
        private Transform _projectilesRoot;
        
        private readonly Dictionary<ProjectileType, ProjectilePool> _projectilePools = new();
        private readonly Dictionary<ProjectileType, string> _projectileAddresses = new()
        {
            { ProjectileType.Default, "DefaultProjectile" },
            { ProjectileType.Explosive, "ExplosiveProjectile" }
        };
        
        private readonly Dictionary<EnemyType, GameObject> _enemyDictionary = new();
        private readonly Dictionary<EnemyType, string> _enemiesAddresses = new()
        {
            { EnemyType.Default, "DefaultEnemy" },
            { EnemyType.Warrior, "WarriorEnemy" }
        };
        
        public GameObject Player { get; private set; }
        public EnemySpawner EnemySpawner { get; private set; }


        public GameFactory(IInstantiator instantiator, IAssetProvider assetProvider, IPersistenceProgressService persistenceProgress, ILevelDescriptionService descriptionService) : base(instantiator)
        {
            _descriptionService = descriptionService;
            _persistenceProgress = persistenceProgress;
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public async Task WarmUp()
        {
            List<Task> tasks = new List<Task>
            {
                LoadEnemySpawner(),
                LoadPlayer(),
                LoadEnemies()
            };

            await Task.WhenAll(tasks);
        }

        private async Task LoadEnemySpawner()
        {
            _enemySpawnerPrefab = await _assetProvider.LoadPersistence<GameObject>(EnemySpawnerAddress);
        }
        
        private async Task LoadPlayer() => 
            _playerPrefab = await _assetProvider.LoadPersistence<GameObject>(PlayerPrefabAddress);
        
        private async Task LoadEnemies()
        {
            foreach (EnemyType type in Enum.GetValues(typeof(EnemyType)))
            {
                if(type == EnemyType.Unknown)
                    continue;
                var prefab = await _assetProvider.LoadPersistence<GameObject>(_enemiesAddresses[type]);
                _enemyDictionary[type] = prefab;
            }
        }
        
        public async Task LoadProjectile(ProjectileType type)
        {
            var prefab = await _assetProvider.LoadPersistence<GameObject>(_projectileAddresses[type]);
            var pool = new ProjectilePool(prefab, type, _projectilesRoot, 20);
            _projectilePools[type] = pool;
        }
        
        public async Task<GameObject> CreatePlayer(Vector3 position)
        {
            Player = Instantiate(_playerPrefab, position, Quaternion.Euler(0,0,0), null);
            
            CharacterAttack attack = Player.GetComponent<CharacterAttack>();
            
            if (attack is ProjectileAttack projectileAttack)
            {
                await projectileAttack.Initialize();
            }
            
            return Player;
        }

        public Task<EnemySpawner> CreateEnemySpawner()
        {
            EnemySpawner = Instantiate(_enemySpawnerPrefab, Vector3.zero, Quaternion.identity, null).GetComponent<EnemySpawner>();
            
            return Task.FromResult(EnemySpawner);
        }
        
        public Projectile GetProjectile(ProjectileType type)
        {
            return _projectilePools[type].Get();
        }

        public async Task<GameObject> SpawnEnemy(EnemySpawnSpot enemySpawnSpot)
        {
            GameObject enemy = null;
            switch (enemySpawnSpot.EnemyType)
            {
                case EnemyType.Unknown:
                    break;
                case EnemyType.Default:
                    enemy = Instantiate(_enemyDictionary[EnemyType.Default], enemySpawnSpot.SpawnPosition, Quaternion.Euler(0,0,0), _enemiesRoot);
                    break;
                case EnemyType.Warrior:
                    enemy = Instantiate(_enemyDictionary[EnemyType.Warrior], enemySpawnSpot.SpawnPosition, Quaternion.Euler(0,0,0), _enemiesRoot);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(enemySpawnSpot.EnemyType), enemySpawnSpot.EnemyType, null);
            }
            
            await InitializeEnemy(enemy, enemySpawnSpot);
            return await Task.FromResult(enemy);
        }

        public void CreateRootParents()
        {
            _enemiesRoot = new GameObject("[Enemies]").transform;
            _projectilesRoot = new GameObject("[Projectiles]").transform;
        }

        private async Task InitializeEnemy(GameObject enemy, EnemySpawnSpot enemySpawnSpot)
        {
            CharacterAttack attack = enemy.GetComponent<CharacterAttack>();
            
            if (attack is ProjectileAttack projectileAttack)
            {
                await projectileAttack.Initialize();
            }

            enemy.GetComponentInChildren<EnemyMovement>().Initialize(enemySpawnSpot);
        }
    }
}
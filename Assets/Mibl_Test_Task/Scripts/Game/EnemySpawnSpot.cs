using Mibl_Test_Task.Scripts.Game;
using UnityEngine;

namespace Game
{
    public class EnemySpawnSpot: MonoBehaviour
    {
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private PatrolType _patrolType;
        [SerializeField] private float _patrolRange;
        
        public Vector3 SpawnPosition => this.transform.position;
        public EnemyType EnemyType => _enemyType;
        public PatrolType PatrolType => _patrolType;
        public float PatrolRange => _patrolRange;
    }
}
using Game;
using Mibl_Test_Task.Scripts.Game.Camera;
using Services.LevelContext;
using UnityEngine;

namespace Infrastructure.LevelContext
{
    public class LevelContextRegister: MonoBehaviour, ILevelContext
    {
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public CameraMovement CameraMovement { get; private set; }

        [field: SerializeField] public EnemySpawnSpot[] EnemySpawnSpots { get; private set; } = new EnemySpawnSpot[] { };

        private void Awake()
        {
            if (CameraMovement == null)
                CameraMovement = Camera.GetComponent<CameraMovement>();
            if(EnemySpawnSpots.Length == 0) 
                EnemySpawnSpots = Object.FindObjectsOfType<EnemySpawnSpot>();
            SingletonLevelContext.Instance.SetInstance(this);
        }

        [ContextMenu("Find")]
        public void Find()
        {
            Camera = Camera.main;
            CameraMovement = Camera.GetComponent<CameraMovement>();
            EnemySpawnSpots = Object.FindObjectsOfType<EnemySpawnSpot>();
        }
    }
}
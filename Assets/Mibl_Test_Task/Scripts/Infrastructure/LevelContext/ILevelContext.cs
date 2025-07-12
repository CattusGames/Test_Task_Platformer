using Game;
using Mibl_Test_Task.Scripts.Game.Camera;
using UnityEngine;

namespace Infrastructure.LevelContext
{
    public interface ILevelContext
    {
        Camera Camera { get; }
        CameraMovement CameraMovement { get; }
        EnemySpawnSpot[] EnemySpawnSpots { get; }
    }
}
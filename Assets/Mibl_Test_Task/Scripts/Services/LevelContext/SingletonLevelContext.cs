using System;
using Game;
using Infrastructure.LevelContext;
using Mibl_Test_Task.Scripts.Game.Camera;
using UnityEngine;

namespace Services.LevelContext
{
    public class SingletonLevelContext: ILevelContext
    {
        public static SingletonLevelContext Instance { get; private set; }

        private ILevelContext _levelContextImplementation;

        public SingletonLevelContext()
        {
            Instance = this;
        }

        public void SetInstance(ILevelContext instance) => _levelContextImplementation = 
            instance ?? throw new ArgumentNullException();
        Camera ILevelContext.Camera => _levelContextImplementation.Camera;
        CameraMovement ILevelContext.CameraMovement => _levelContextImplementation.CameraMovement;
        EnemySpawnSpot[] ILevelContext.EnemySpawnSpots => _levelContextImplementation.EnemySpawnSpots;
    }
}
using System;
using Mibl_Test_Task.Scripts.StaticData;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Level")]
    public partial class ScriptableAccident : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private AddressableSceneReference _sceneReference;

        [SerializeField] private float _startDistance;
        
        [LabelText("Finish Conditions")]
        [EnumToggleButtons]
        public FinishConditions _finishConditions;

        [ShowIf(nameof(IsKillCountCondition))]
        [LabelText("Enemies Count")]
        public int _killTarget;

        [ShowIf(nameof(IsDistanceCondition))]
        [LabelText("Distance (m)")]
        public float _distanceTarget;

        public string SceneName
        {
            get => _sceneReference.SceneAddress;
            set => _sceneReference.SceneAddress = value;
        }

        public int ID => _id;
        public float LevelLength
        {
            get =>
                _distanceTarget;
            set => _distanceTarget = value;
        }
        
        public float StartDistance
        {
            get => 
                _startDistance;
            set => _startDistance = value;
        }
        
        public FinishConditions FinishConditions => _finishConditions;

        public LevelData ExportData() =>
            new LevelData
            {
                Id = _id,
                StartDistance = _startDistance,
                SceneName = SceneName,
                FinishConditions = _finishConditions,
                KillTarget = _killTarget,
                DistanceTarget = _distanceTarget,
            };
    
        public void ImportData(LevelData levelData)
        {
            if(_id != levelData.Id)
            {
                Debug.LogError($"You are trying load config with id {levelData.Id} for level with id {_id}");
                return;
            }

            _id = levelData.Id;
            _startDistance = levelData.StartDistance;
            _distanceTarget = levelData.DistanceTarget;
            SceneName = levelData.SceneName;
            _finishConditions = levelData.FinishConditions;
            _killTarget = levelData.KillTarget;
            _distanceTarget = levelData.DistanceTarget;
        }
        
        private bool IsKillCountCondition() => _finishConditions.HasFlag(FinishConditions.KillCount);
        private bool IsDistanceCondition() => _finishConditions.HasFlag(FinishConditions.Distance);
    }
    
    [Serializable]
    public class LevelData : IJsonable
    {
        public int Id;
        public float StartDistance;
        public string SceneName;
        public FinishConditions FinishConditions;
        public int KillTarget;
        public float DistanceTarget;
    }
    
    [Serializable]
    public class AddressableSceneReference
    {
        public string SceneAddress;
    }
}
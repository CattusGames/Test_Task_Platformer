using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Game", fileName = "GameConfig", order = 0)]
    public class GameStaticData : ScriptableObject
    {
        public string InitialScene = "Initial";
    }
}

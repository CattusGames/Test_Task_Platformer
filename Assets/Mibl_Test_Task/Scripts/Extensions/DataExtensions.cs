using UnityEngine;

namespace Extensions
{
    public static class DataExtensions
    {
        public static T ToDeserialize<T>(this string json) => 
            JsonUtility.FromJson<T>(json);

        public static string ToJson(this object self) => 
            JsonUtility.ToJson(self, false);
    }
}